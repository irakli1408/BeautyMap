using BeautyMap.Domain.Common.Contract;

namespace BeautyMap.Application.Common.NamedLanguages
{
    public class TextLocaleModel : ILocales
    {
        public string Text { get; set; }
        public int LanguageId { get; set; }
    }
}
