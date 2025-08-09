using BeautyMap.Application.Persistence;
using BeautyMap.Application.Tools.ValidationHelper;
using BeautyMap.Domain.Entities.Account;
using BeautyMap.NotificationManager.Enums;
using BeautyMap.Application.Handlers.Account.Commands.ResetPasswordConfirmation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeautyMap.Application.Handlers.Account.Commands.PasswordReset
{
    public class PasswordResetCommandHandler : IRequestHandler<ResetPasswordConfirmationCommand,Unit>
    {
        private readonly UserManager<UserEntity> userManager;
        private readonly IBeautyMapDbContext db;
        public PasswordResetCommandHandler(UserManager<UserEntity> userManager, IBeautyMapDbContext db)
        {
            this.userManager = userManager;
            this.db = db;
        }
        public async Task<Unit> Handle(ResetPasswordConfirmationCommand request, CancellationToken cancellationToken)
        {
            var userConfirmationCode = db.UserConfirmationCodes
                .Include(x => x.User)
                .Include(x => x.NotificationType)
                .FirstOrDefault(x => x.Code == request.ConfirmationCode &&
                x.DeleteDate == null &&
                x.User.DeleteDate == null &&
                   (x.NotificationType.Id == (int)SendNotificationTypes.ResetPassword ||
                    x.NotificationType.Id == (int)SendNotificationTypes.SetAdminPassword));

            if (userConfirmationCode.ExpirationDate < DateTime.UtcNow)
                throw new Exception("Confirmation Code Already Expired.");

            var passwordToken = await userManager.GeneratePasswordResetTokenAsync(userConfirmationCode.User);

            await userManager.PasswordValidation(userConfirmationCode.User, request.NewPassword);

            userConfirmationCode.Delete();

            await userManager.ResetPasswordAsync(userConfirmationCode.User, passwordToken, request.NewPassword);

            return Unit.Value;
        }
    }
}
