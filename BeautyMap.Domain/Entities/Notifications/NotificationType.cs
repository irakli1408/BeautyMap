using BeautyMap.Domain.Common.BaseEntities;
using BeautyMap.Domain.Entities.Account;

namespace BeautyMap.Domain.Entities.Notifications
{
    public class NotificationType : BaseEntity
    {
        public string Name { get; set; }

        #region Relationship
        public virtual ICollection<NotificationLocale> Locales { get; set; }
        public virtual ICollection<UserConfirmation> UserConfirmations { get; set; }
        #endregion
    }
}
