using MediatR;
using BeautyMap.Application.Common;
using BeautyMap.Common.Models;

namespace BeautyMap.Application.Handlers.Admin.UserManagers.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<GetUserByIdQueryResponse>
    {
        public string Id { get; set; }
    }
    public class GetUserByIdQueryResponse : UserCommonModel
    {
        public string Id { get; set; }
        public List<NamedData<string>> Roles { get; set; }
        public bool IsBlocked { get; set; }
    }
}
