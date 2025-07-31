namespace BeautyMap.Application.Common.Contracts.Paging
{
    public class PagingModel : IPaging
    {
        private int page = 1;
        private int offset = 10;

        public int Page
        {
            get => page;
            set => page = value <= 0 ? 1 : value;
        }

        public int Offset
        {
            get => offset;
            set => offset = value <= 0 ? 10 : value;
        }
    }
}