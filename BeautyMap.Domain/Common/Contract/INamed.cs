namespace BeautyMap.Domain.Common.Contract
{
    public interface INamed : ILocales
    {
        public string Name { get; set; }
    }
}
