using BeautyMap.Application.Common.NamedLanguages;
using BeautyMap.Application.Persistence;
using BeautyMap.Common.CurrentState;
using BeautyMap.Common.Enums;
using BeautyMap.Domain.Common.Contract;
using Microsoft.EntityFrameworkCore;

namespace BeautyMap.Application.Tools.Managers.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly IBlogLikeDbContext db;
        private readonly ICurrentStateService stateService;
        private int? cachedLanguageId;

        public LanguageService(IBlogLikeDbContext db, ICurrentStateService stateService)
        {
            this.db = db;
            this.stateService = stateService;
        }

        public int GetPreferredLanguageId()
            => GetLanguageId();

        #region RetrieveLocalized string

        public string RetrieveLocalizedName<T>(ICollection<T> locales) where T : INamed, IDeletable
        {
            var localizedEntry = RetrieveLocalized(locales);
            return localizedEntry?.Name;
        }
        public string RetrieveLocalizedName<T>(ICollection<T> locales, bool forAdmin = false) where T : INamed, IDeletable
        {
            var localizedEntry = RetrieveLocalized(locales, forAdmin);
            return localizedEntry?.Name;
        }
        public string RetrieveLocalizedName<T>(IEnumerable<T> locales) where T : INamed, IDeletable
        {
            var localizedEntry = RetrieveLocalized(locales);
            return localizedEntry?.Name;
        }

        public string RetrieveLocalizedText<T>(ICollection<T> locales) where T : IText, IDeletable
        {
            var localizedEntry = RetrieveLocalized(locales);

            return localizedEntry?.Text;
        }
        public string RetrieveLocalizedText<T>(IEnumerable<T> locales) where T : IText, IDeletable
        {
            var localizedEntry = RetrieveLocalized(locales);

            return localizedEntry?.Text;
        }
        public string RetrieveLocalizedNamedText<T>(ICollection<T> locales, string propertyName) where T : INamed, IText, IDeletable
        {
            var localizedEntry = RetrieveLocalized(locales);

            return propertyName switch
            {
                nameof(INamed.Name) => localizedEntry?.Name,
                nameof(IText.Text) => localizedEntry?.Text,
                _ => default
            };
        }
        public string RetrieveLocalizedNamedText<T>(IEnumerable<T> locales) where T : INamed, IText, IDeletable
        {
            var localizedEntry = RetrieveLocalized(locales);

            return localizedEntry?.Text;
        }
        #endregion

        #region RetrieveNamedLanguage List
        public List<NamedLanguage> RetrieveNamedLanguageList<T>(IEnumerable<T> locales) where T : INamed, IDeletable
        {
            return RetrieveLocaleModelList(locales, language => new NamedLanguage
            {
                LanguageId = language.LanguageId,
                Name = language.Name
            });
        }
        public List<TextNamedLanguage> RetrieveTextNamedLanguageList<T>(IEnumerable<T> locales) where T : INamed, IText, IDeletable
        {
            return RetrieveLocaleModelList(locales, language => new TextNamedLanguage
            {
                LanguageId = language.LanguageId,
                Name = language.Name,
                Text = language.Text
            });
        }

        public List<TextLocaleModel> RetrieveTextLocaleModelList<T>(IEnumerable<T> locales) where T : IText, IDeletable
        {
            return RetrieveLocaleModelList(locales, language => new TextLocaleModel
            {
                LanguageId = language.LanguageId,
                Text = language.Text
            });
        }

        public List<NotificationsLocaleModel> RetrieveNotificationList<T>(IEnumerable<T> locales) where T : INotificationLocale, IDeletable
        {
            return RetrieveLocaleModelList(locales, language => new NotificationsLocaleModel
            {
                LanguageId = language.LanguageId,
                Body = language.Body,
                Title = language.Title
            });
        }
        #endregion


        #region Private

        #region RetrieveLocaleModelList

        public List<TModel> RetrieveLocaleModelList<TLocale, TModel>(
            IEnumerable<TLocale> locales,
            Func<TLocale, TModel> projectionFunction)
            where TLocale : IDeletable, ILocales
            where TModel : ILocales, new()
        {
            var positions = db.Languages.AsNoTracking();

            var models = locales
                .Where(locale => locale.DeleteDate == null)
                .Select(projectionFunction)
                .ToList() ?? [];

            return [.. models];
        }

        #endregion

        #region RetrieveLocalized
        private T RetrieveLocalized<T>(IEnumerable<T> locales, bool forAdmin = false) where T : ILocales, IDeletable
        {
            if (locales == null || !locales.Any())
                return default;

            if (forAdmin)
            {
                var customlanguageId = GetLanguageId("en-US");

                var customlocalizedEntry = locales.FirstOrDefault(loc => loc.LanguageId == customlanguageId && loc.DeleteDate == null) ??
                                     locales.FirstOrDefault(loc => loc.LanguageId == (int)LanguageType.Georgian);

                return customlocalizedEntry;
            }

            var languageId = cachedLanguageId.HasValue ?
                             cachedLanguageId :
                             GetLanguageId();

            var localizedEntry = locales.FirstOrDefault(loc => loc.LanguageId == languageId && loc.DeleteDate == null) ??
                                 locales.FirstOrDefault(loc => loc.LanguageId == (int)LanguageType.Georgian);

            return localizedEntry;
        }

        #endregion

        private int GetLanguageId(string languageCode = null)
        {
            if (String.IsNullOrEmpty(languageCode))
            {
                var customLanguageId = db.Languages.AsNoTracking().FirstOrDefault(l => l.LanguageCode == stateService.AcceptedLanguage).Id;

                cachedLanguageId = customLanguageId == 0 ? (int)LanguageType.Georgian : customLanguageId;
                return cachedLanguageId.Value;
            }

            var languageId = db.Languages.AsNoTracking().FirstOrDefault(l => l.LanguageCode == languageCode).Id;
            return languageId == 0 ? (int)LanguageType.Georgian : languageId;
        }
        #endregion
    }
}
