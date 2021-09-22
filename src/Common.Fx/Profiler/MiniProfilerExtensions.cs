using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Fx.Profiler
{
    public static class MiniProfilerExtensions
    {
        public static void AddTheMiniProfiler(this IServiceCollection services)
        {
            //refs => https://miniprofiler.com/
            //Once you configure the RouteBasePath property, we are able access:
            //profiler/results => the current request
            //profiler/results-index => a list of all requests
            //profiler/results-list => a list of all requests as JSON
            services.AddMiniProfiler(options =>
                {
                    options.RouteBasePath = "/profiler";
                })
                .AddEntityFramework();
        }

        public static void UseTheMiniProfiler(this IApplicationBuilder app)
        {
            app.UseMiniProfiler();
        }
    }
}
