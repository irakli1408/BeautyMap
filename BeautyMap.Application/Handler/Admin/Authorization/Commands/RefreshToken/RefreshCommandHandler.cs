using BeautyMap.Application.Persistence;
using BeautyMap.Application.Tools.Managers.Token;
using BeautyMap.Application.Handlers.Account.Commands.UserLogin;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace BeautyMap.Application.Handlers.Account.Commands.RefreshToken
{
    public class RefreshCommandHandler : IRequestHandler<RefreshCommand, TokenApiResponseModel>
    {
        private readonly IBeautyMapDbContext db;
        private readonly ITokenService tokenService;
        public RefreshCommandHandler(IBeautyMapDbContext db, ITokenService tokenService)
        {
            this.db = db;
            this.tokenService = tokenService;
        }
        public async Task<TokenApiResponseModel> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var principal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            var UserId = principal.Identity?.Name;

            await tokenService.IsAuthorized(UserId);

            var user = await db.Users.FirstAsync(x => x.Id == UserId, cancellationToken);

            var newAccessToken = await tokenService.CreateTokenAsync(user.Email);
            var newRefreshToken = tokenService.GenerateRefreshToken();

            await tokenService.SaveRefreshToken(request.RefreshToken, newRefreshToken, user, cancellationToken);

            return new TokenApiResponseModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            };
        }
    }
}
