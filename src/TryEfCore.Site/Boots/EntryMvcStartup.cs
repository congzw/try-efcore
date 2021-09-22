using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Common;
using Common.Fx.ApiDoc;
using Common.Fx.DI;
using Common.Fx.Profiler;
using Common.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TryEfCore.Libs.Data;

namespace TryEfCore.Site.Boots
{
    public static class EntryMvcStartup
    {
        public static IServiceCollection MainConfigureService(this IServiceCollection services, IWebHostEnvironment webEnv, IConfiguration config)
        {
            var reflectHelper = ReflectHelper.Instance;
            var assemblies = reflectHelper.GetAssembliesFrom(AppDomain.CurrentDomain.BaseDirectory, new[] { "Common", "TryEfCore" }).ToArray();

            //Common
            services.AddTheCommon(assemblies);

            //DI
            services.AddTheScrutor(assemblies);

            //ApiDoc
            services.AddTheApiDoc(assemblies);

            //mini profiler
            services.AddTheMiniProfiler();
            
            //TheDbContext, SharedModelBinder
            services.AddTestDbContext();
            
            return services;
        }
        
        public static IApplicationBuilder MainConfigure(this IApplicationBuilder app, IWebHostEnvironment webEnv)
        {
            app.UseTheCommon();
            
            if (webEnv.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMyStaticFiles(webEnv);
            
            app.UseTheMiniProfiler();
            
            app.UseTheMiniProfiler();
            
            return app;
        }

        public static IMvcCoreBuilder ConfigTheMvcCore(this IMvcCoreBuilder mvcCoreBuilder)
        {
            ////配置动态编译
            //mvcCoreBuilder.AddRazorRuntimeCompilation();

            //配置异常处理
            //mvcCoreBuilder.AddMvcOptions(options =>
            //{
            //    options.Filters.Add<MyExceptionFilter>();
            //});
            
            //mvcCoreBuilder.AddTheMvcFilter();
            
            return mvcCoreBuilder;
        }

        private static void UseMyStaticFiles(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                //解决linux的大小写问题
                var physicalFileProvider = env.WebRootFileProvider;
                var myPhysicalFileProvider = new MyPhysicalFileProvider(physicalFileProvider);
                myPhysicalFileProvider.InitMap("", physicalFileProvider);
                env.WebRootFileProvider = myPhysicalFileProvider;
            }

            app.UseDefaultFiles(new DefaultFilesOptions() { DefaultFileNames = new List<string>() { "index.html" } });
            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = new FileExtensionContentTypeProvider
                {
                    Mappings =
                    {
                        [".vue"] = "text/html"
                    }
                }
            });
        }
    }
}
