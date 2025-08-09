using BeautyMap.Application.Common;
using BeautyMap.Application.Tools.Extensions;
using BeautyMap.Common.Enums;
using BeautyMap.Common.Models;
using BeautyMap.Domain.Entities.Account;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeautyMap.Application.Handlers.Admin.UserManagers.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagedData<GetUsersQueryResponse>>
    {
        private readonly UserManager<UserEntity> userManager;

        public GetUsersQueryHandler(UserManager<UserEntity> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<PagedData<GetUsersQueryResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var query = userManager.Users
                .AsNoTracking()
                .Include(x => x.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(x => x.RoleLocales)
                .Where(x => x.DeleteDate == null);

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                var searchKey = request.SearchKey.ToLower();
                query = query.Where(x =>
                        x.FirstName.ToLower().Contains(searchKey) ||
                        x.LastName.ToLower().Contains(searchKey) ||
                        x.Email.ToLower().Contains(searchKey));
            }

            if (request.IsBlocked.HasValue)
            {
                query = query.Where(x => request.IsBlocked.Value ? x.LockoutEnd > DateTime.UtcNow : x.LockoutEnd <= DateTime.UtcNow);
            }

            var users = await query
                .OrderByDescending(x => x.CreateDate)
                .Select(user => new GetUsersQueryResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsBlocked = user.LockoutEnd != null && user.LockoutEnd > DateTime.UtcNow,
                    Roles = user.UserRoles.Select(ur => new NamedData<string>
                    {
                        Id = ur.Role.Id,
                        Name = ur.Role.RoleLocales.FirstOrDefault(x => x.LanguageId == (int)LanguageType.Georgian).Name,
                    })
                }).ToPagedDataAsync(request.Page, request.Offset, cancellationToken);

            return users;
        }
    }
}
