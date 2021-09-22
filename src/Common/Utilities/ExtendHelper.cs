using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Utilities
{
    public interface IExtendHelper
    {
        string MyJoin(IEnumerable<string> values);
        List<string> MySplit(string value);
        bool MyEquals(string value, string value2, StringComparison comparison = StringComparison.OrdinalIgnoreCase);
        bool MyContains(IEnumerable<string> values, string toCheck, StringComparison comparison = StringComparison.OrdinalIgnoreCase);
        string MyFind(IEnumerable<string> values, string toCheck, StringComparison comparison = StringComparison.OrdinalIgnoreCase);
    }

    public class ExtendHelper : IExtendHelper
    {
        #region auto resolve from di or default

        [LazySingleton]
        public static IExtendHelper Instance => LazySingleton.Instance.Resolve(() => new ExtendHelper());

        #endregion

        public char[] Splitter { get; set; } = {',', '，'};

        public string MyJoin(IEnumerable<string> values)
        {
            return string.Join(',', values.Select(x => x));
        }

        public List<string> MySplit(string value)
        {
            return value.Split(Splitter, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        
        public bool MyEquals(string value, string value2, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            var valueFix = string.Empty;
            if (!string.IsNullOrWhiteSpace(value))
            {
                valueFix = value.Trim();
            }

            var value2Fix = string.Empty;
            if (!string.IsNullOrWhiteSpace(value2))
            {
                value2Fix = value2.Trim();
            }

            return valueFix.Equals(value2Fix, comparison);
        }

        public bool MyContains(IEnumerable<string> values, string toCheck, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            foreach (var value in values)
            {
                if (value.MyEquals(toCheck, comparison))
                {
                    return true;
                }
            }
            return false;
        }

        public string MyFind(IEnumerable<string> values, string toCheck, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            foreach (var value in values)
            {
                if (value.MyEquals(toCheck, comparison))
                {
                    return value;
                }
            }
            return null;
        }
    }
}
