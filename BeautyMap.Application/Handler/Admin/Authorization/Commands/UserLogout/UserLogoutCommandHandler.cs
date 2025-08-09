using BeautyMap.Application.Persistence;
using MediatR;

namespace BeautyMap.Application.Handlers.Account.Commands.UserLogout
{
    public class UserLogoutCommandHandler : IRequestHandler<UserLogoutCommand, Unit>
    {
        private readonly IBeautyMapDbContext db;
        public UserLogoutCommandHandler(IBeautyMapDbContext db)
        {
            this.db = db;
        }
        public async Task<Unit> Handle(UserLogoutCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = db.UserRefreshTokens.FirstOrDefault(x => x.UserId == request.UserId && x.DeleteDate == null);

            refreshToken?.Delete();

            await db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
