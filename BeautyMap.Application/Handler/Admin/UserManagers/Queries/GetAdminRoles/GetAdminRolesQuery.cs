using BeautyMap.Application.Common.Models;
using MediatR;

namespace BeautyMap.Application.Handlers.Admin.UserManagers.Queries.GetAdminRoles
{
    public class GetAdminRolesQuery : IRequest<List<GetAdminRolesResponse>>
    { }

    public class GetAdminRolesResponse : NamedData<string>
    { }
}
