namespace BeautyMap.Application.Common.Contracts.Paging
{
    public interface IPaging
    {
        public int Page { get; set; }
        public int Offset { get; set; }
    }
}
