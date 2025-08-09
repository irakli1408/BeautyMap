using BeautyMap.Common.Models;
using MediatR;

namespace BeautyMap.Application.Handlers.Admin.UserManagers.Queries.GetRoles
{
    public class GetRolesQuery : IRequest<List<GetRolesResponse>>
    { }

    public class GetRolesResponse : NamedData<string>
    { }
}
