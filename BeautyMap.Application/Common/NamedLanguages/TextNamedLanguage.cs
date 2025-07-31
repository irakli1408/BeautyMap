using BeautyMap.Domain.Common.Contract;

namespace BeautyMap.Application.Common.NamedLanguages
{
    public class TextNamedLanguage : IText, INamed
    {
        public string Name { get; set; }
        public int LanguageId { get; set; }
        public string Text { get; set; }
    }
}
