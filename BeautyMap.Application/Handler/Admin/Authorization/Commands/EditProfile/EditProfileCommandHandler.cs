using BeautyMap.Application.Persistence;
using BeautyMap.Application.Tools.Extesnsions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyMap.Application.Handlers.Account.Commands.EditProfile
{
    public class EditProfileCommandHandler : IRequestHandler<EditProfileCommand, Unit>
    {
        private readonly IBeautyMapDbContext db;
        public EditProfileCommandHandler(IBeautyMapDbContext db)
        {
            this.db = db;
        }
        public async Task<Unit> Handle(EditProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == request.UserId && x.DeleteDate == null, cancellationToken);

            if (user == null)
                throw new Exception("User Does Not Exist Or is Deleted.");

            if (user.Email != request.Model.Email)
            {
                await db.IsEmailRegistered(request.Model.Email);
            }

            user.FirstName = request.Model.FirstName;
            user.LastName = request.Model.LastName;
            user.Email = request.Model.Email;

            await db.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
