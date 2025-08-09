using FluentValidation;

namespace BeautyMap.Application.Handlers.Account.Queries.CheckEmail
{
    public class CheckEmailQueryValidator : AbstractValidator<EmailExistsQuery>
    {
        public CheckEmailQueryValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotNull()
                .NotEmpty()
                .MaximumLength(250);
        }
    }
}
