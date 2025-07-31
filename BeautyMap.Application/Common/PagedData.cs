using BeautyMap.Application.Common.Contracts.Paging;

namespace BeautyMap.Application.Common
{
    public class PagedData<T> : PagingModel
    {
        public IQueryable<T> Data { get; set; }
        public int TotalItemCount { get; set; }
        public int PageCount { get; set; }
    }

    public class PagedDataList<T> : PagingModel
    {
        public List<T> Data { get; set; }
        public int TotalItemCount { get; set; }
        public int PageCount { get; set; }
    }
}
