using BeautyMap.Application.Persistence;
using BeautyMap.NotificationManager.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyMap.Application.Handlers.Account.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Unit>
    {
        private readonly IBeautyMapDbContext db;
        public ConfirmEmailCommandHandler(IBeautyMapDbContext db)
        {
            this.db = db;
        }
        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var userConfirmationCode = await db.UserConfirmationCodes
                .Include(x => x.User)
                .Include(x => x.NotificationType)
                .FirstOrDefaultAsync(x => x.Code == request.ConfirmationCode &&
                x.DeleteDate == null &&
                x.User.DeleteDate == null &&
                x.NotificationType.Id == (int)SendNotificationTypes.ConfirmEmail, cancellationToken);

            if (userConfirmationCode.ExpirationDate < DateTime.UtcNow)
                throw new Exception("Confirmation Code Expired.");

            userConfirmationCode.User.EmailConfirmed = true;
            userConfirmationCode.User.IsActive = true;

            userConfirmationCode.Delete();

            await db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
