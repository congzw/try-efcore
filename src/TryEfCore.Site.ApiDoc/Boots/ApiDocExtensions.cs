using System;
using System.IO;
using System.Linq;
using Common;
using Common.Fx.ApiDoc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace TryEfCore.Site.ApiDoc.Boots
{
    internal static class ApiDocSetup
    {
        internal static IServiceCollection AddApiDoc(this IServiceCollection services, ApiDocInfoRegistry registry, ILogger logger = null)
        {
            if (registry == null) throw new ArgumentNullException(nameof(registry));

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory ?? string.Empty;

            services.AddSwaggerGen(opt =>
            {
                //注意!
                //实际的调用顺序是[UseSwaggerUI, AddSwaggerGen]
                var apiDocInfos = registry.ApiDocInfos;
                if (apiDocInfos == null)
                {
                    throw new InvalidOperationException("ApiDocInfoRegistry试用线必须初始化！");
                }

                TryLogMessage(logger, $"AddSwaggerGen => {apiDocInfos.Count} {apiDocInfos.Select(x => x.Name).MyJoin()}");
                
                //load xml comments
                var xmlFiles = apiDocInfos.Select(x => x.XmlFile).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                foreach (var xmlFile in xmlFiles)
                {
                    var xmlFilePath = Path.Combine(baseDirectory, xmlFile);
                    if (File.Exists(xmlFilePath))
                    {
                        opt.IncludeXmlComments(xmlFilePath);
                    }
                }

                //load groups
                foreach (var apiDocInfo in apiDocInfos)
                {
                    opt.SwaggerDoc(apiDocInfo.Name, new OpenApiInfo()
                    {
                        Title = apiDocInfo.Title,
                        Version = apiDocInfo.Version,
                        Description = apiDocInfo.Description
                    });
                }
            });
            return services;
        }

        internal static IApplicationBuilder UseApiDoc(this IApplicationBuilder app, ApiDocInfoRegistry registry, ILogger logger = null)
        {
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                ////serve the Swagger UI at the root (http://localhost:<port>/)
                //c.RoutePrefix = string.Empty;

                //switch "~/swagger/index.html" to "~/SwaggerHide/index.html", so now we use the only mvc "~/ApiDoc" entry
                opt.RoutePrefix = "SwaggerHide";
                var apiDocInfos = registry.ApiDocInfos;

                TryLogMessage(logger, $"UseSwaggerUI => {apiDocInfos.Count} {apiDocInfos.Select(x => x.Name).MyJoin()}");

                foreach (var apiDocInfo in apiDocInfos)
                {
                    opt.SwaggerEndpoint(apiDocInfo.Endpoint, apiDocInfo.Name);
                }
            });
            return app;
        }

        private static void TryLogMessage(ILogger logger, string message)
        {
            if (logger == null)
            {
                //LogHelper.Instance.Info(message);
                Console.WriteLine(message);
            }
            else
            {
                logger.LogInformation(message);
            }
        }
    }
}
