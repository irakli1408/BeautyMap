using BeautyMap.Application.Persistence;
using BeautyMap.Application.Tools.Extesnsions;
using BeautyMap.Application.Tools.Managers.Notifications;
using BeautyMap.Domain.Entities.Account;
using BeautyMap.NotificationManager.Enums;
using MediatR;

namespace BeautyMap.Application.Handlers.Account.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly IBeautyMapDbContext db;
        private readonly INotificationBuilder notificationBuilder;
        private readonly string confirmationCode;
        public ResetPasswordCommandHandler(IBeautyMapDbContext db, INotificationBuilder notificationBuilder)
        {
            this.db = db;
            this.notificationBuilder = notificationBuilder;
            confirmationCode = StringExtensions.GenerateAccessCode();
        }
        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await db.CheckUser(request.Email);
            if (user == null)
                throw new Exception("User Does Not Exist Or Is Deleted.");

            var userCode = user.UserConfirmationCodes?.FirstOrDefault(x => x.UserId == user.Id &&
                                                   x.NotificationTypeId == (int)SendNotificationTypes.ResetPassword &&
                                                   x.DeleteDate == null);

            if (userCode != null)
            {
                if (userCode?.CreateDate.AddMinutes(2) > DateTime.UtcNow)
                    throw new Exception("Configmration Code Link Already Sended.");

                userCode.Delete();
            }

            var userConfirmationCode = new UserConfirmation(user.Id, confirmationCode)
            {
                NotificationTypeId = (int)SendNotificationTypes.ResetPassword
            };

            db.UserConfirmationCodes.Add(userConfirmationCode);

            await db.SaveChangesAsync(cancellationToken);

            await notificationBuilder.BuildNotificationAsync(user, SendNotificationTypes.ResetPassword, confirmationCode);

            return Unit.Value;
        }
    }
}
