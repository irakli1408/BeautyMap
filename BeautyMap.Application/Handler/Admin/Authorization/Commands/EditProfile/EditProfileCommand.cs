using MediatR;
using BeautyMap.Application.Common.Contracts.NeedsAuthentication;

namespace BeautyMap.Application.Handlers.Account.Commands.EditProfile
{
    public class EditProfileCommand : Authentication, IRequest<Unit>
    {
        public EditProfileCommandModel Model { get; set; }
    }
    public class EditProfileCommandModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
