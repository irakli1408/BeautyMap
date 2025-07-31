using BeautyMap.Domain.Entities.Account;
using BeautyMap.Domain.Entities.Admin.Languages;
using BeautyMap.Domain.Entities.Authentication;
using BeautyMap.Domain.Entities.Files;
using BeautyMap.Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using File = BeautyMap.Domain.Entities.Files.File;


namespace BeautyMap.Application.Persistence
{
    public interface IBlogLikeDbContext
    {
        #region Tables
        #region User
        DbSet<UserEntity> Users { get; set; }

        #endregion
        #region Role
        DbSet<Role> Roles { get; set; }
        DbSet<RoleLocale> RoleLocales { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        #endregion

        #region File
        DbSet<File> Files { get; set; }
        DbSet<FileType> FileTypes { get; set; }
        #endregion
        #region UserRefreshTokens
        DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        #endregion
        #region UserConfirmationCodes
        DbSet<UserConfirmation> UserConfirmationCodes { get; set; }
        #endregion
        #region Notifications
        DbSet<Notification> Notifications { get; set; }
        DbSet<NotificationLocale> NotificationLocales { get; set; }
        DbSet<NotificationType> NotificationTypes { get; set; }
        #endregion

        #region Language
        DbSet<Language> Languages { get; set; }
        #endregion
        #endregion

        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
