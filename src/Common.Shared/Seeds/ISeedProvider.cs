using Common.Fx.DI;

namespace Common.Shared.Seeds
{
    public interface ISeedProvider : IAutoInjectAsTransient
    {
        int Order { get; set; }
        void Setup(SeedRegistry registry);
        void Seed(SeedRegistry registry, string category);
    }
}