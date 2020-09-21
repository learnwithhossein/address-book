using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Common
{
    public class PagedList<TEntity> : List<TEntity> where TEntity : class
    {
        private const int DefaultPageSize = 8;
        private const int MaxPageSize = 20;

        public Pagination Pagination { get; set; }

        private PagedList(IEnumerable<TEntity> items, int pageNumber, int pageSize, int totalCount)
        {
            Pagination = new Pagination(pageNumber, pageSize, totalCount);

            AddRange(items);
        }

        public static async Task<PagedList<TEntity>> Create(IQueryable<TEntity> source, int pageNumber, int pageSize)
        {
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;
            pageSize = pageSize < 1 ? DefaultPageSize : pageSize;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            var totalCount = await source.CountAsync();

            var items = await source.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedList<TEntity>(items, pageNumber, pageSize, totalCount);
        }
    }
}
