namespace BeautyMap.Domain.Common.Contract
{
    public interface IDeletable
    {
        DateTime? DeleteDate { get; protected set; }
        void Delete();
    }
}
