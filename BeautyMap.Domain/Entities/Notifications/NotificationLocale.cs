using BeautyMap.Domain.Common.BaseEntities;
using BeautyMap.Domain.Common.Contract;
using BeautyMap.Domain.Entities.Admin.Languages;

namespace BeautyMap.Domain.Entities.Notifications
{
    public class NotificationLocale : TrackedEntity, INotificationLocale
    {
        public string Title { get; set; }
        public string Body { get; set; }

        #region Relationship
        public int LanguageId { get; set; }
        public virtual Language Languages { get; set; }
        public int NotificationTypeId { get; set; }
        public virtual NotificationType NotificationType { get; set; }
        #endregion
    }
}
