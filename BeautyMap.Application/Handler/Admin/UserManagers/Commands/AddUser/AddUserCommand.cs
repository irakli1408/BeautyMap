using MediatR;
using BeautyMap.Application.Common;

namespace BeautyMap.Application.Handlers.Admin.UserManagers.Commands.AddUser
{
    public class AddUserCommand : UserCommonModel, IRequest<Unit>
    {
        public string RoleId { get; set; }
    }
}
