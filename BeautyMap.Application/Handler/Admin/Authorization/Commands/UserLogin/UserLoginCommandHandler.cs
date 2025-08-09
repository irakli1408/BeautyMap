using BeautyMap.Application.Persistence;
using BeautyMap.Application.Tools.Managers.Notifications;
using BeautyMap.Application.Tools.Managers.Token;
using BeautyMap.Domain.Entities.Account;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace BeautyMap.Application.Handlers.Account.Commands.UserLogin
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, TokenApiResponseModel>
    {
        private readonly UserManager<UserEntity> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IBeautyMapDbContext db;
        private readonly ITokenService tokenService;
        private UserEntity user;
        private readonly DateTime now;
        private readonly INotificationBuilder notificationBuilder;
        public UserLoginCommandHandler(UserManager<UserEntity> userManager,
            IBeautyMapDbContext db,
            ITokenService tokenService,
            INotificationBuilder notificationBuilder,
            RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.db = db;
            this.tokenService = tokenService;
            now = DateTime.UtcNow;
            this.notificationBuilder = notificationBuilder;
            this.roleManager = roleManager;
        }

        public async Task<TokenApiResponseModel> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            await ValidateUserAsync(request, cancellationToken);

            var token = await tokenService.CreateTokenAsync(user.Email);
            var refresherToken = tokenService.GenerateRefreshToken();

            await tokenService.SaveRefreshToken(refresherToken, user, cancellationToken);

            var roleIds = user.UserRoles.Select(x => x.RoleId);

            return new TokenApiResponseModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refresherToken,
                RoleId = roleIds
            };
        }
        #region Validate
        private async Task ValidateUserAsync(UserLoginCommand login, CancellationToken cancellationToken)
        {
            user = await db.Users.Include(x => x.UserRoles).FirstOrDefaultAsync(x => x.Email == login.Email && x.DeleteDate == null, cancellationToken);

            if (user == null || !await userManager.CheckPasswordAsync(user, login.Password))
                throw new Exception("Wrong Email Or Password.");
            //ToDo: tu undat ro dailoqos comment unda aexsnas.
            //if (user.LockoutEnd.HasValue && user.LockoutEnd.Value.DateTime > DateTime.UtcNow)
            //{
            //    var minutesUntilUnblocked = (user.LockoutEnd.Value.DateTime - DateTime.UtcNow).TotalMinutes;
            //    if (minutesUntilUnblocked <= 1)
            //        minutesUntilUnblocked = 1;
            //    else
            //        minutesUntilUnblocked = Math.Ceiling(minutesUntilUnblocked);
            //    await errorManager.ThrowError(ErrorEnum.AccountIsBlocked, minutesUntilUnblocked.ToString("F0"));
            //}

            //if (!await userManager.CheckPasswordAsync(user, login.Password))
            //{
            //    user.AccessFailedCount++;

            //    var lockouts = await db.UserLockTypes.Select(x => x).ToListAsync(cancellationToken);

            //    var currentLockout = lockouts.FirstOrDefault(ult => ult.FailCount == user.AccessFailedCount);

            //    if (currentLockout != null)
            //    {
            //        await SaveAndThrowError(currentLockout);
            //    }
            //    else
            //    {
            //        var lastLockout = lockouts.LastOrDefault();
            //        if (user.AccessFailedCount % 5 == 0)
            //        {
            //            await SaveAndThrowError(lastLockout);
            //        }

            //    }

            //    await userManager.UpdateAsync(user);
            //    await errorManager.ThrowError(ErrorEnum.WrongEmailOrPassword);
            //}

            user.LockoutEnd = null;
            user.AccessFailedCount = 0;
        }

        //private async Task SaveAndThrowError(UserLockType lockType)
        //{
        //    user.LockoutEnd = now.AddMinutes(lockType.Duration);
        //    await userManager.UpdateAsync(user);
        //    var minutesUntilUnblocked = (user.LockoutEnd.Value.DateTime - DateTime.UtcNow).TotalMinutes;
        //    await notificationBuilder.BuildNotificationAsync(user, SendNotificationTypesEnum.AccountIsBlocked, minutesUntilUnblocked.ToString("F0"));
        //    await errorManager.ThrowError(ErrorEnum.AccountIsBlocked, minutesUntilUnblocked.ToString("F0"));
        //}
        #endregion
    }
}