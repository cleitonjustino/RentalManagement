using Minio.DataModel;
using MongoFramework.Linq;

namespace RentalManagement.Application.QueryStack.Helpers
{
    public class PagedList<T> : List<T> where T : class
    {
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            AddRange(items);
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var count = await source.CountAsync();

            if (count < pageSize)
                pageSize = count;
            if(count > 0)
            {
                var items = await source.Skip(pageSize * (pageNumber - 1))
                   .Take(pageSize).ToListAsync(cancellationToken);
                return new PagedList<T>(items, count, pageNumber, pageSize);
            }

            return new PagedList<T>(new List<T>(), 0, pageNumber, pageSize);
        }
    }
}
