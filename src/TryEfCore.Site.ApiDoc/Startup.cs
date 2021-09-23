using System;
using Common.Fx.ApiDoc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.Modules;
using TryEfCore.Site.ApiDoc.Boots;

namespace TryEfCore.Site.ApiDoc
{
    /// <summary>
    /// �Ƴ�����󣬱�֤Api��ע�ᣬ���ܵ��õ�
    /// </summary>
    public class Startup : StartupBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Startup> _logger;
        private readonly ApiDocInfoRegistry _apiDocInfoRegistry;

        public override int Order => 100;
        public override int ConfigureOrder => 100;

        public Startup(IConfiguration configuration, ILogger<Startup> logger, ApiDocInfoRegistry apiDocInfoRegistry)
        {
            _configuration = configuration;
            _logger = logger;
            _apiDocInfoRegistry = apiDocInfoRegistry;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddApiDoc(_apiDocInfoRegistry, _logger);
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            base.Configure(builder, routes, serviceProvider);
            builder.UseApiDoc(_apiDocInfoRegistry, _logger);
        }
    }
}