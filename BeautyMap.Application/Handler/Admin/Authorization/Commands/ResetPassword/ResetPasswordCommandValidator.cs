using FluentValidation;

namespace BeautyMap.Application.Handlers.Account.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(request => request.Email).EmailAddress().NotNull().NotEmpty();
        }
    }
}
