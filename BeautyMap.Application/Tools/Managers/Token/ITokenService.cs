using BeautyMap.Domain.Entities.Account;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BeautyMap.Application.Tools.Managers.Token
{
    public interface ITokenService
    {
        string GenerateRefreshToken();
        Task<JwtSecurityToken> CreateTokenAsync(string email);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        Task IsAuthorized(string userId);

        Task SaveRefreshToken(string oldRefreshToken, string refreshToken, UserEntity user, CancellationToken cancellationToken);
        Task SaveRefreshToken(string refreshToken, UserEntity user, CancellationToken cancellationToken);
    }
}
