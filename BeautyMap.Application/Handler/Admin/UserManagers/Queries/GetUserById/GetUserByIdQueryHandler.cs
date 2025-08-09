using BeautyMap.Application.Persistence;
using BeautyMap.Application.Tools.Extensions;
using BeautyMap.Common.Enums;
using BeautyMap.Common.Models;
using MediatR;

namespace BeautyMap.Application.Handlers.Admin.UserManagers.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdQueryResponse>
    {
        private readonly IBeautyMapDbContext db;

        public GetUserByIdQueryHandler(IBeautyMapDbContext db)
        {
            this.db = db;
        }
        public async Task<GetUserByIdQueryResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await db.CheckUser(request.Id);

            var model = new GetUserByIdQueryResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = user.UserRoles.Select(r => new NamedData<string>
                {
                    Id = r.RoleId,
                    Name = r.Role.RoleLocales.FirstOrDefault(x => x.LanguageId == (int)LanguageType.Georgian).Name
                }).ToList(),
                IsBlocked = user.LockoutEnd > DateTime.UtcNow,
            };

            return model;
        }
    }
}
