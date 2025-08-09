using MediatR;

namespace BeautyMap.Application.Handlers.Account.Commands.UserLogout
{
    public class UserLogoutCommand : IRequest<Unit>
    {
        public string UserId { get; set; }
    }
}
