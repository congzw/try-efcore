using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Fx.ApiDoc
{
    public static class ApiDocSetupExtensions
    {
        public static IServiceCollection AddTheApiDoc(this IServiceCollection services, params Assembly[] appAssemblies)
        {
            //auto setup by reflection
            var apiDocInfoRegistry = ApiDocInfoRegistry.Instance;
            apiDocInfoRegistry.Setup(appAssemblies);
            apiDocInfoRegistry.Apply();
            services.AddSingleton(apiDocInfoRegistry);

            return services;
        }
    }
}