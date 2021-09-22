using System.Collections.Generic;
using System.Linq;
using Common.Fx.DI;

namespace Common.Shared.Seeds
{
    public interface ISeedService : IAutoInjectAsTransient
    {
        void RunSeed(SeedRegistry registry, string category);
    }
    
    class SeedService : ISeedService
    {
        private readonly List<ISeedProvider> _seedProviders;

        public SeedService(IEnumerable<ISeedProvider> seedProviders)
        {
            _seedProviders = seedProviders.OrderBy(x => x.Order).ToList();
        }

        public void RunSeed(SeedRegistry registry, string category)
        {
            var isSetupApplied = registry.IsSetupApplied();
            if (!isSetupApplied)
            {
                registry.ApplySetup(_seedProviders);
            }
            
            foreach (var seedProvider in _seedProviders)
            {
                seedProvider.Seed(registry, category);
            }
        }
    }
}