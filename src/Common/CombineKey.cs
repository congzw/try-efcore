using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class CombineKey
    {
        public List<string> Keys { get; set; } = new List<string>();
        public string ToCombineKey()
        {
            return string.Join(',', Keys);
        }

        public static CombineKey Create(params string[] keys)
        {
            return new CombineKey { Keys = keys.ToList() };
        }
    }
}
