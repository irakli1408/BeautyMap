using BeautyMap.Domain.Common.Contract;

namespace BeautyMap.Domain.Common.BaseEntities
{
    public abstract class LocaleEntity : TrackedEntity<int>, INamed
    {
        public int LanguageId { get; set; }
        public string Name { get; set; }
    }

    public abstract class LocaleTextEntity : TrackedEntity, IText
    {
        public int LanguageId { get; set; }
        public string Text { get; set; }
    }
    public abstract class LocaleTextNameEntity : TrackedEntity, IText, INamed
    {
        public int LanguageId { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
    }
}
