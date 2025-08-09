using BeautyMap.Application.Tools.ValidationHelper;
using BeautyMap.Domain.Entities.Account;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BeautyMap.Application.Handlers.Account.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly UserManager<UserEntity> userManager;
        public ChangePasswordCommandHandler(UserManager<UserEntity> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);

            await userManager.PasswordValidation(user, request.Model.NewPassword);

            var result = await userManager.ChangePasswordAsync(user, request.Model.OldPassword, request.Model.NewPassword);

            if (!result.Succeeded)
                throw new Exception("Wrong Password.");

            return Unit.Value;
        }
    }
}
