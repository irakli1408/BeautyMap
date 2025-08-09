using MediatR;

namespace BeautyMap.Application.Handlers.Account.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<Unit>
    {
        public string Email { get; set; }
    }
}
