namespace BeautyMap.Domain.Common.Contract
{
    public interface INotificationLocale : ILocales
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
