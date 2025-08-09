using BeautyMap.Application.Persistence;
using BeautyMap.Domain.Entities.Account;
using BeautyMap.Domain.Enums.Role;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeautyMap.Application.Handlers.Admin.UserManagers.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IBeautyMapDbContext db;
        private readonly UserManager<UserEntity> userManager;
        public DeleteUserCommandHandler(IBeautyMapDbContext db, UserManager<UserEntity> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == request.Id && x.DeleteDate == null, cancellationToken);

            if (user == null)
                throw new Exception("User Does Not Exist Or It is Deleted");

            var roles = await userManager.GetRolesAsync(user);

            user.Delete();

            await db.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
