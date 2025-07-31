using BeautyMap.Domain.Common.Contract;

namespace BeautyMap.Application.Common.NamedLanguages
{
    public class NamedLanguage : INamed
    {
        public string Name { get; set; }
        public int LanguageId { get; set; }
    }
}
