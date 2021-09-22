using Common.Fx.DI;
using TryEfCore.Libs.Data;

namespace TryEfCore.Libs.Core.Demos
{
    public partial interface IDemoService : IAutoInjectAsTransient
    {
    }

    partial class DemoService : IDemoService
    {
        private readonly TestDbContext _dbContext;

        public DemoService(TestDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
