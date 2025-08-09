using FluentValidation;

namespace BeautyMap.Application.Handlers.Account.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.Model.OldPassword)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.Model.NewPassword)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.Model.NewPasswordConfirm)
                .NotNull()
                .NotEmpty()
                .MinimumLength(8)
                .Equal(x => x.Model.NewPassword);
        }
    }
}
