using BeautyMap.Application.Persistence;
using BeautyMap.Domain.Entities.Account;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BeautyMap.Application.Handlers.Admin.UserManagers.Commands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Unit>
    {
        private readonly UserManager<UserEntity> userManager;
        private readonly IBeautyMapDbContext db;
        private readonly IConfiguration configuration;
        private readonly INotificationBuilder notificationBuilder;
        private readonly string confirmationCode;
        public AddUserCommandHandler(UserManager<UserEntity> userManager,
                                     IBeautyMapDbContext db,
                                     IConfiguration configuration,
                                     INotificationBuilder notificationBuilder)
        {
            this.userManager = userManager;
            this.db = db;
            this.configuration = configuration;
            this.notificationBuilder = notificationBuilder;
            confirmationCode = StringExtensions.GenerateAccessCode();
        }
        public async Task<Unit> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.RegisterUser(
                new UserCommonModel
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName
                },
            configuration["DefaultPassword"],
            request.RoleId);

            var userConfirmationCode = new UserConfirmation(user.Id, confirmationCode)
            {
                NotificationTypeId = (int)SendNotificationTypes.SetAdminPassword
            };

            db.UserConfirmationCodes.Add(userConfirmationCode);

            await db.SaveChangesAsync(cancellationToken);

            await notificationBuilder.BuildNotificationAsync(user, SendNotificationTypes.SetAdminPassword, confirmationCode);

            return Unit.Value;
        }
    }
}
