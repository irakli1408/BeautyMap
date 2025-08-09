using FluentValidation;

namespace BeautyMap.Application.Handlers.Account.Commands.UserLogin
{
    public class UserLoginCommandValidation : AbstractValidator<UserLoginCommand>
    {
        public UserLoginCommandValidation()
        {
            RuleFor(x => x.Email).EmailAddress().NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}
