using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Paging
{
    public class PageInfo : IPageInfo
    {
        internal int From { get; set; }
        public int Index { get; set; }
        public int Size { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious => Index - From > 0;
        public bool HasNext => Index - From + 1 < TotalPages;

        public PageInfo(int index, int size, int totalCount, int totalPages, int indexFrom = 1)
        {
            var pageIndexSize = PageAutoFix.Create(index, size, indexFrom);
            From = pageIndexSize.From;
            Index = pageIndexSize.Index;
            Size = pageIndexSize.Size;
            TotalCount = totalCount;
            TotalPages = totalPages;
        }
    }

    public static class PageInfoExtensions
    {
        public static PageInfo GetPageInfo<T>(this IEnumerable<T> source, int index, int size, int indexFrom = 1)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var totalCount = source.Count();
            var pageIndexSize = PageAutoFix.Create(index, size, indexFrom);
            return CreatePageInfo(totalCount, pageIndexSize.Index, pageIndexSize.Size, pageIndexSize.From);
        }
        public static PageInfo GetPageInfo<T>(this IQueryable<T> source, int index, int size, int indexFrom = 1)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var totalCount = source.Count();
            var pageIndexSize = PageAutoFix.Create(index, size, indexFrom);
            return CreatePageInfo(totalCount, pageIndexSize.Index, pageIndexSize.Size, pageIndexSize.From);
        }
        private static PageInfo CreatePageInfo(int totalCount, int index, int size, int indexFrom = 1)
        {
            var totalPages = (int)Math.Ceiling(totalCount / (double)size);
            var pageInfo = new PageInfo(index, size, totalCount, totalPages, indexFrom);
            return pageInfo;
        }
    }
}
