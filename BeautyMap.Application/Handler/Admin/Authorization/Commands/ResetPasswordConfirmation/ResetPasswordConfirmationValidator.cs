using FluentValidation;

namespace BeautyMap.Application.Handlers.Account.Commands.ResetPasswordConfirmation
{
    public class ResetPasswordConfirmationValidator : AbstractValidator<ResetPasswordConfirmationCommand>
    {
        public ResetPasswordConfirmationValidator()
        {
            RuleFor(request => request.ConfirmationCode).NotNull().NotEmpty();
            RuleFor(request => request.NewPasswordConfirm)
            .NotNull()
            .NotEmpty()
            .MinimumLength(8)
            .Equal(request => request.NewPassword);
        }
    }
}
