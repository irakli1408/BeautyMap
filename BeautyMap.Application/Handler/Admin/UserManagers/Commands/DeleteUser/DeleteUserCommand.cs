using MediatR;

namespace BeautyMap.Application.Handlers.Admin.UserManagers.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public string Id { get; set; }
    }
}
