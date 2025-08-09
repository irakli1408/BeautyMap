using BeautyMap.Application.Persistence;
using BeautyMap.Common.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyMap.Application.Handlers.Admin.UserManagers.Queries.GetRoles
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<GetRolesResponse>>
    {
        private readonly IBeautyMapDbContext db;
        public GetRolesQueryHandler(IBeautyMapDbContext db)
            => this.db = db;

        public async Task<List<GetRolesResponse>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
            => await db.RoleLocales
            .AsNoTracking()
            .Where(x => x.LanguageId == (int)LanguageType.Georgian)
            .Select(x => new GetRolesResponse
            {
                Id = x.RoleId,
                Name = x.Name,
            }).ToListAsync(cancellationToken);

    }
}
