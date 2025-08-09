using MediatR;
using BeautyMap.Application.Common.Contracts.NeedsAuthentication;

namespace BeautyMap.Application.Handlers.Account.Queries.GetProfile
{
    public record GetProfileQuery : AuthenticationRecord, IRequest<ProfileResponseModel>
    { }

    public class ProfileResponseModel
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
