using BeautyMap.Application.Common.Contracts.NeedsAuthentication;
using MediatR;

namespace BeautyMap.Application.Handlers.Account.Commands.ChangePassword
{
    public class ChangePasswordCommand : Authentication, IRequest<Unit>
    {
        public ChangePasswordCommandModel Model { get; set; }
    }
    public class ChangePasswordCommandModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirm { get; set; }
    }
}
