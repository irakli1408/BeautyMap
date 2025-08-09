using BeautyMap.Application.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyMap.Application.Handlers.Admin.UserManagers.Queries.GetAdminRoles
{
    public class GetAdminRolesQueryHandler : IRequestHandler<GetAdminRolesQuery, List<GetAdminRolesResponse>>
    {
        private readonly IBeautyMapDbContext db;
        public GetAdminRolesQueryHandler(IBeautyMapDbContext db)
            => this.db = db;

        public async Task<List<GetAdminRolesResponse>> Handle(GetAdminRolesQuery request, CancellationToken cancellationToken)
            => await db.RoleLocales
            .AsNoTracking()
            .Select(x => new GetAdminRolesResponse
            {
                Id = x.RoleId,
                Name = x.Name,
            }).ToListAsync(cancellationToken);
    }
}
