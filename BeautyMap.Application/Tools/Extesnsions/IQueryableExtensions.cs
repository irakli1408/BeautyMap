using BeautyMap.Application.Common;
using BeautyMap.Domain.Common.Contract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BeautyMap.Application.Tools.Extesnsions
{
    public static class IQueryableExtensions
    {
        public static PagedData<T> ToPagedData<T>(this IQueryable<T> query, int page, int offset)
        {
            var count = query.Count();
            var list = query.Skip((page - 1) * offset).Take(offset);
            var data = new PagedData<T>()
            {
                Data = list,
                TotalItemCount = count,
                Page = page,
                Offset = offset,
                PageCount = count % offset == 0 ? count / offset : count / offset + 1
            };
            return data;
        }
        public static async Task<PagedData<T>> ToPagedDataAsync<T>(this IQueryable<T> query, int page, int offset, CancellationToken cancellationToken = default)
        {
            var count = await query.CountAsync(cancellationToken);
            var list = query.Skip((page - 1) * offset).Take(offset);
            var data = new PagedData<T>()
            {
                Data = list,
                TotalItemCount = count,
                Page = page,
                Offset = offset,
                PageCount = count % offset == 0 ? count / offset : count / offset + 1
            };
            return data;
        }
        public static PagedData<T> ToQueryablePagedData<T>(this List<T> list, int page, int offset)
        {
            var query = list.AsQueryable();
            return query.ToPagedData(page, offset);
        }
        public static PagedData<T> ToQueryablePagedData<T>(this IEnumerable<T> list, int page, int offset)
        {
            var query = list.AsQueryable();
            return query.ToPagedData(page, offset);
        }

        public static IQueryable<T> WhereNotDeleted<T>(this IQueryable<T> source) where T : IDeletable
        {
            Expression<Func<T, bool>> predicate = x => x.DeleteDate == null;
            return source.Where(predicate);
        }
        public static IQueryable<T> WhereDeleted<T>(this IQueryable<T> source) where T : IDeletable
        {
            Expression<Func<T, bool>> predicate = x => x.DeleteDate != null;
            return source.Where(predicate);
        }
    }
}
