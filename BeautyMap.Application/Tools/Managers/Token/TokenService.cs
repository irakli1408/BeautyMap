using BeautyMap.Application.Persistence;
using BeautyMap.Common.CurrentState;
using BeautyMap.Domain.Entities.Account;
using BeautyMap.Domain.Entities.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BeautyMap.Application.Tools.Managers.Token
{
    internal class TokenService : ITokenService
    {
        private readonly UserManager<UserEntity> userManager;
        private readonly IConfiguration configuration;
        private UserEntity user;
        private readonly IBlogLikeDbContext db;

        private readonly ICurrentStateService currentStateService;

        public TokenService(UserManager<UserEntity> userManager,
            IConfiguration configuration,
            IBlogLikeDbContext db,
            ICurrentStateService currentStateService)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.db = db;
            this.currentStateService = currentStateService;
        }

        #region AccessToken
        public async Task<JwtSecurityToken> CreateTokenAsync(string email)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(email);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims, 30.ToString());
            return tokenOptions;
        }

        #region Private
        private SigningCredentials GetSigningCredentials()
        {
            var jwtConfig = configuration.GetSection("TokenValidationParameters");
            var key = Encoding.UTF8.GetBytes(jwtConfig["Key"]!);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims(string email)
        {
            user = await db.Users
                .FirstOrDefaultAsync(x => x.Email == email && x.DeleteDate == null);
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Id),
                new(ClaimTypes.Email, user.Email),
                new("EmailVerified", user.EmailConfirmed.ToString()),
                new("PhoneNumberVerified", user.PhoneNumberConfirmed.ToString()),
                new("FullName", $"{user.FirstName} {user.LastName}"),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims, string tokenExpiresIn)
        {
            var jwtSettings = configuration.GetSection("TokenValidationParameters");
            var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings["ValidIssuer"],
                audience: jwtSettings["ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(Convert.ToDouble(tokenExpiresIn)),
                signingCredentials: signingCredentials
            );
            return tokenOptions;
        }
        #endregion
        #endregion

        #region RefreshToken
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtConfig = configuration.GetSection("TokenValidationParameters");
            var Key = Encoding.UTF8.GetBytes(jwtConfig["Key"]!);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = (securityToken as JwtSecurityToken)!;
            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception("Invalid Token");
            }
            return principal;
        }
        #endregion

        #region SaveRefreshToken
        public async Task SaveRefreshToken(string oldRefreshToken, string refreshToken, UserEntity user, CancellationToken cancellationToken)
        {
            var authorized = await db.UserRefreshTokens
                .FirstOrDefaultAsync(x =>
                x.UserId == user.Id &&
                x.DeleteDate == null &&
                x.RefreshToken == oldRefreshToken, cancellationToken);

            if (authorized == null)
                throw new Exception("Invalid Refresh token");

            authorized.RefreshToken = refreshToken;

            await db.SaveChangesAsync(cancellationToken);
        }
        public async Task SaveRefreshToken(string refreshToken, UserEntity user, CancellationToken cancellationToken)
        {
            var authorized = await db.UserRefreshTokens
                .FirstOrDefaultAsync(x =>
                x.UserId == user.Id &&
                x.DeleteDate == null, cancellationToken);

            authorized?.Delete();

            CreateNewUserRefreshToken(user, refreshToken);

            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task IsAuthorized(string userId)
        {
            var tokenExists = await db.UserRefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId && x.DeleteDate == null);

            if (tokenExists == null)
                throw new Exception("Unauthorized!");
        }

        #region Private
        private void CreateNewUserRefreshToken(UserEntity user, string refreshToken)
        {
            db.UserRefreshTokens.Add(new UserRefreshToken
            {
                UserId = user.Id,
                RefreshToken = refreshToken,
            });
        }
        #endregion

        #endregion
    }
}
