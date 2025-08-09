using FluentValidation;

namespace BeautyMap.Application.Handlers.Account.Commands.EditProfile
{
    public class EditProfileCommandValidator : AbstractValidator<EditProfileCommand>
    {
        public EditProfileCommandValidator()
        {
            RuleFor(x => x.Model.FirstName)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Model.LastName)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Model.Email)
                .EmailAddress()
                .NotNull()
                .NotEmpty();
        }
    }
}
