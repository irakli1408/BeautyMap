using FluentValidation;

namespace BeautyMap.Application.Handlers.Admin.UserManagers.Commands.EditUser
{
    public class EditUserCommandValidator : AbstractValidator<EditUserCommand>
    {
        public EditUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull();
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

            RuleFor(x => x.RoleId)
                .NotEmpty()
                .NotNull();
        }
    }
}
