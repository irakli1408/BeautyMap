using BeautyMap.Application.Common.NamedLanguages;
using BeautyMap.Domain.Common.Contract;

namespace BeautyMap.Application.Tools.Managers.Languages
{
    public interface ILanguageService
    {
        int GetPreferredLanguageId();

        string RetrieveLocalizedName<T>(ICollection<T> locales) where T : INamed, IDeletable;
        string RetrieveLocalizedName<T>(ICollection<T> locales, bool forAdmin = false) where T : INamed, IDeletable;
        string RetrieveLocalizedName<T>(IEnumerable<T> locales) where T : INamed, IDeletable;
        string RetrieveLocalizedText<T>(ICollection<T> locales) where T : IText, IDeletable;
        string RetrieveLocalizedText<T>(IEnumerable<T> locales) where T : IText, IDeletable;

        string RetrieveLocalizedNamedText<T>(ICollection<T> locales, string propertyName) where T : INamed, IText, IDeletable;
        string RetrieveLocalizedNamedText<T>(IEnumerable<T> locales) where T : INamed, IText, IDeletable;

        List<NamedLanguage> RetrieveNamedLanguageList<T>(IEnumerable<T> locales) where T : INamed, IDeletable;
        List<TextNamedLanguage> RetrieveTextNamedLanguageList<T>(IEnumerable<T> locales) where T : INamed, IText, IDeletable;
        List<TextLocaleModel> RetrieveTextLocaleModelList<T>(IEnumerable<T> locales) where T : IText, IDeletable;
        List<NotificationsLocaleModel> RetrieveNotificationList<T>(IEnumerable<T> locales) where T : INotificationLocale, IDeletable;
    }
}
