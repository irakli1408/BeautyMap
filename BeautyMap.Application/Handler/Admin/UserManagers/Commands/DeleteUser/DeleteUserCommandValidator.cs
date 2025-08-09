using FluentValidation;

namespace BeautyMap.Application.Handlers.Admin.UserManagers.Commands.DeleteUser
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull()
                .Must(id => id != "a6730b4d-da93-4e0b-bef6-6d28fbdd4d4f")
                .WithMessage("ს.ადმინის წაშლა შეუძლებელია.");
        }
    }
}
