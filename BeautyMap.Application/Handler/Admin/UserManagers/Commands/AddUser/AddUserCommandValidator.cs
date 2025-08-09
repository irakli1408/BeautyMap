using FluentValidation;

namespace BeautyMap.Application.Handlers.Admin.UserManagers.Commands.AddUser
{
    public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.LastName)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty()
                .NotNull();
        }
    }
}
