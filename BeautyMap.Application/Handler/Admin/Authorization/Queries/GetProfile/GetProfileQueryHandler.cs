using BeautyMap.Application.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyMap.Application.Handlers.Account.Queries.GetProfile
{
    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ProfileResponseModel>
    {
        private readonly IBeautyMapDbContext db;

        public GetProfileQueryHandler(IBeautyMapDbContext db)
        {
            this.db = db;
        }

        public async Task<ProfileResponseModel> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var userProfile = await db.Users
                .AsNoTracking()
                .Include(x => x.UserRoles)
                .Where(u => u.Id == request.UserId && u.DeleteDate == null)
                .FirstOrDefaultAsync(cancellationToken);

            var result = new ProfileResponseModel()
            {
                Id = userProfile.Id,
                RoleId = userProfile.UserRoles.FirstOrDefault().RoleId,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Email = userProfile.Email,
                CreateDate = userProfile.CreateDate
            };

            return result;
        }
    }
}
