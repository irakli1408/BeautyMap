using MediatR;

namespace BeautyMap.Application.Handlers.Account.Commands.ResetPasswordConfirmation
{
    public class ResetPasswordConfirmationCommand : IRequest<Unit>
    {
        public string ConfirmationCode { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirm { get; set; }
    }
}
