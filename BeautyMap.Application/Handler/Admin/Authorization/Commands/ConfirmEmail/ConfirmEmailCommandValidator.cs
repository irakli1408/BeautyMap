using FluentValidation;

namespace BeautyMap.Application.Handlers.Account.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.ConfirmationCode)
                .NotNull()
                .NotEmpty();
        }
    }
}
