using System;
using Common.Shared.Boots;
using Common.Shared.Seeds;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TryEfCore.Libs.Data;

namespace TryEfCore.Site.Boots
{
    public class EntryOrchardStartup : MyStartupBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EntryOrchardStartup> _logger;

        public EntryOrchardStartup(IConfiguration configuration, ILogger<EntryOrchardStartup> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            //todo: load dev module by config
            base.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            base.Configure(builder, routes, serviceProvider);
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var theDbContext = scope.ServiceProvider.GetService<TestDbContext>();
                theDbContext.Database.EnsureDeleted();
                theDbContext.Database.EnsureCreated();

                var seedService = scope.ServiceProvider.GetService<ISeedService>();
                var seedRegistry = new SeedRegistry();
                
                //seed init data
                seedService.RunSeed(seedRegistry, SeedCategory.Init);

                //seed test data
                seedService.RunSeed(seedRegistry, SeedCategory.Test);
            }
        }
    }
}
