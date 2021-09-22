using System;

namespace Common.Utilities
{
    public class InstanceDesc
    {
        public string DeclaringType { get; set; }
        public int Hashcode { get; set; }
        public string GetDesc()
        {
            if (string.IsNullOrWhiteSpace(DeclaringType))
            {
                return "<NULL>";
            }
            return $"<{DeclaringType}:{Hashcode}>";
        }
        public static InstanceDesc Create(object instance, Type declaringType = null)
        {
            var desc = new InstanceDesc();
            if (instance == null)
            {
                return desc;
            }

            if (declaringType == null)
            {
                declaringType = instance.GetType();
            }

            desc.DeclaringType = declaringType.GetFriendlyNameWithNamespace();
            desc.Hashcode = instance.GetHashCode();
            return desc;
        }
    }
}
