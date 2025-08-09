using MediatR;

namespace BeautyMap.Application.Handlers.Account.Commands.UserLogin
{
    public class UserLoginCommand : IRequest<TokenApiResponseModel>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class TokenApiResponseModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public IEnumerable<string> RoleId { get; set; }
    }
}
