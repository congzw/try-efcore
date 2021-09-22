using System.Collections.Generic;

namespace Common.Paging
{
    public interface IPageArgs
    {
        int Index { get; set; }
        int Size { get; set; }
    }

    public interface IPageInfo : IPageArgs
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious { get; }
        public bool HasNext { get; }
    }

    public interface IPageResult<T> : IPageInfo
    {
        IEnumerable<T> Items { get; set; }
    }
}
