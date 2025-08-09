using MediatR;
using BeautyMap.Application.Handlers.Account.Commands.UserLogin;

namespace BeautyMap.Application.Handlers.Account.Commands.RefreshToken
{
    public class RefreshCommand : IRequest<TokenApiResponseModel>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
