using MediatR;
using BeautyMap.Application.Common;
using BeautyMap.Application.Common.Contracts.Paging;
using BeautyMap.Common.Models;

namespace BeautyMap.Application.Handlers.Admin.UserManagers.Queries.GetUsers
{
    public class GetUsersQuery : PagingModel, IRequest<PagedData<GetUsersQueryResponse>>
    {
        public string SearchKey { get; set; }
        public string RoleId { get; set; }
        public bool? IsBlocked { get; set; } = null;
    }

    public class GetUsersQueryResponse : UserCommonModel
    {
        public string Id { get; set; }
        public IEnumerable<NamedData<string>> Roles { get; set; }
        public bool IsBlocked { get; set; }
    }
}
