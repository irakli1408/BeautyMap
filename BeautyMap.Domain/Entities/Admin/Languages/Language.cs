using BeautyMap.Domain.Common.BaseEntities;
using BeautyMap.Domain.Entities.Account;
using BeautyMap.Domain.Entities.Notifications;

namespace BeautyMap.Domain.Entities.Admin.Languages
{
    public class Language : BaseEntity
    {
        public string Name { get; set; }
        public string Abbreviature { get; set; }
        public string LanguageCode { get; set; }

        #region Relationship

        public virtual ICollection<RoleLocale> RoleLocales { get; set; }
        public virtual ICollection<NotificationLocale> NotificationLocales { get; set; }

        #endregion
    }
}
