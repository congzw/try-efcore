using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Paging
{
    public class PageResult<T> : PageInfo
    {
        public IEnumerable<T> Items { get; set; }

        private readonly PageInfo _pageInfo;
        public PageInfo GetPageInfo()
        {
            return _pageInfo;
        }

        public PageResult(PageInfo pageInfo, IEnumerable<T> items, int from = 1) : base(pageInfo.Index, pageInfo.Size, pageInfo.TotalCount, pageInfo.TotalPages, from)
        {
            _pageInfo = pageInfo;
            Items = items;
        }
    }

    public static class PageResultExtensions
    {
        public static PageResult<T> GetPageResult<T>(this IEnumerable<T> source, int index, int size, int indexFrom = 1)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var list = source.ToList();
            var pageInfo = list.GetPageInfo(index, size, indexFrom);
            var items = list.Skip((pageInfo.Index - pageInfo.From) * pageInfo.Size).Take(pageInfo.Size).ToList();
            return new PageResult<T>(pageInfo, items);
        }
        public static PageResult<T> GetPageResult<T>(this IQueryable<T> source, int index, int size, int indexFrom = 1)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var pageInfo = source.GetPageInfo(index, size, indexFrom);
            var items = source.Skip((pageInfo.Index - pageInfo.From) * pageInfo.Size).Take(pageInfo.Size).ToList();
            return new PageResult<T>(pageInfo, items);
        }
    }
}
