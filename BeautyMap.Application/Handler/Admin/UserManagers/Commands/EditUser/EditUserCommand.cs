using MediatR;
using BeautyMap.Application.Common;

namespace BeautyMap.Application.Handlers.Admin.UserManagers.Commands.EditUser
{
    public class EditUserCommand : UserCommonModel, IRequest<Unit>
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
    }
}
