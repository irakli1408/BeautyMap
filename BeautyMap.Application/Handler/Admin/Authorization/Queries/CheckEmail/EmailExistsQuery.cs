using MediatR;

namespace BeautyMap.Application.Handlers.Account.Queries.CheckEmail
{
    public class EmailExistsQuery : IRequest<bool>
    {
        public string Email { get; set; }
    }
}
