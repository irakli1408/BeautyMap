using BeautyMap.Application.Persistence;
using BeautyMap.Application.Tools.Extesnsions;
using BeautyMap.Domain.Entities.Account;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BeautyMap.Application.Handlers.Admin.UserManagers.Commands.EditUser
{
    public class EditUserCommandHandler : IRequestHandler<EditUserCommand, Unit>
    {
        private readonly UserManager<UserEntity> userManager;
        private readonly IBeautyMapDbContext db;
        public EditUserCommandHandler(UserManager<UserEntity> userManager, IBeautyMapDbContext db)
        {
            this.userManager = userManager;
            this.db = db;
        }

        public async Task<Unit> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            var user = await db.CheckUser(request.Id);
            if (user == null)
                throw new Exception("User Does Not Exist Or It Is Deleted");

            if (user.Email != request.Email)
            {
                await db.IsEmailRegistered(request.Email);
            }

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                var removeRoleResult = await userManager.RemoveFromRoleAsync(user, role);
                UserExtensions.ThrowIfIdentityResultFailed(removeRoleResult);
            }

            var roleEnum = UserRoleHelperExtensions.GetRoleCodeEnum(request.RoleId).ToString();

            var addToRoleResult = await userManager.AddToRoleAsync(user, roleEnum);
            UserExtensions.ThrowIfIdentityResultFailed(addToRoleResult);

            Guid guid = Guid.NewGuid();
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            if (request.Email != user.Email)
            {
                var emailFirstPart = request.Email.Split('@')[0];

                user.UserName = emailFirstPart + guid;
                user.NormalizedUserName = (emailFirstPart + guid).ToUpper();
                user.OriginalUserName = emailFirstPart;
                user.Email = request.Email;
                user.EmailConfirmed = false;
                user.IsActive = false;
            }

            await db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
