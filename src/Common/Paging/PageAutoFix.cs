namespace Common.Paging
{
    public class PageAutoFix
    {
        public int Index { get; set; }
        public int Size { get; set; }
        public int From { get; set; }
        public static PageAutoFix Create(int index, int size, int from = 1)
        {
            var pageIndexSize = new PageAutoFix();
            if (from < 1)
            {
                from = 1;
            }
            if (index < 1)
            {
                index = 1;
            }
            if (size <= 0)
            {
                size = 10;
            }

            pageIndexSize.Index = index;
            pageIndexSize.Size = size;
            pageIndexSize.From = from;
            return pageIndexSize;
        }
    }
}