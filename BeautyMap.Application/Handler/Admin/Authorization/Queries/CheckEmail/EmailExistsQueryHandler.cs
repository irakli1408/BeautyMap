using BeautyMap.Application.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BeautyMap.Application.Handlers.Account.Queries.CheckEmail
{
    public class EmailExistsQueryHandler : IRequestHandler<EmailExistsQuery, bool>
    {
        private readonly IBeautyMapDbContext db;

        public EmailExistsQueryHandler(IBeautyMapDbContext db)
           => this.db = db;

        public async Task<bool> Handle(EmailExistsQuery request, CancellationToken cancellationToken)
            => await db.Users.AsNoTracking().AnyAsync(u => u.Email == request.Email && u.DeleteDate == null, cancellationToken);
    }
}
