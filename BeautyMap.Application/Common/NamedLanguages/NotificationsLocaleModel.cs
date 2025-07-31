using BeautyMap.Domain.Common.Contract;

namespace BeautyMap.Application.Common.NamedLanguages
{
    public class NotificationsLocaleModel : INotificationLocale
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int LanguageId { get; set; }
    }
}
