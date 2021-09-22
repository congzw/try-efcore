using System.Collections.Generic;

namespace Common.ReflectSetups
{
    #region how to use ReflectSetupRegistry

    public class DemoRegistry : ReflectSetupRegistry<DemoRegistry>
    {
        public List<string> Records { get; set; } = new List<string>();
    }

    public interface IDemoRegistrySetup : ISetupByReflect<DemoRegistry>
    {
    }

    public class DemoRegistrySetup1 : IDemoRegistrySetup
    {
        [ReflectMethod]
        public void Setup(DemoRegistry registry)
        {
            registry.Records.Add(this.GetType().FullName);
        }
    }

    public class DemoRegistrySetup2 : IDemoRegistrySetup
    {
        [ReflectMethod]
        public void Setup(DemoRegistry registry)
        {
            registry.Records.Add(this.GetType().FullName);
        }
    }

    public class DemoRegistrySetup3
    {
        [ReflectMethod(Order = 4)]
        public void Foo(DemoRegistry registry)
        {
            registry.Records.Add(this.GetType().FullName);
        }
    }

    public class DemoRegistrySetup4
    {
        [ReflectMethod(Order = 3)]
        public static void Bar(DemoRegistry registry)
        {
            registry.Records.Add(typeof(DemoRegistrySetup4).FullName);
        }
    }

    #endregion
}
