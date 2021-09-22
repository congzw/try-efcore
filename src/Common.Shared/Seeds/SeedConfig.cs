using System;

namespace Common.Shared.Seeds
{
    public class SeedConfig
    {
        public bool RecreateDb { get; set; }
        //public bool RecreateDb { get; set; } = true;
        public bool SeedTest { get; set; } = true;

        private static readonly SeedConfig Default = new SeedConfig();
        public static Func<SeedConfig> GetConfig = () => Default;
    }
}