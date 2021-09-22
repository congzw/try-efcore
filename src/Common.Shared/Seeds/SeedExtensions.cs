using System;

namespace Common.Shared.Seeds
{
    public static class SeedExtensions
    {
        public static Seed GetSeed(this SeedRegistry registry, string category, Type seedType, bool autoCreate)
        {
            return registry.GetSeed(category, seedType.FullName, autoCreate);
        }
        
        public static Seed GetInitSeed(this SeedRegistry registry,  Type seedType, bool autoCreate)
        {
            return registry.GetSeed(SeedCategory.Init, seedType, autoCreate);
        }
        public static Seed GetTestSeed(this SeedRegistry registry,  Type seedType, bool autoCreate)
        {
            return registry.GetSeed(SeedCategory.Test, seedType, autoCreate);
        }
    }
}