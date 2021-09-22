using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TryEfCore.Libs.Data
{
    public static class TestDbContextStartup
    {
        public static IServiceCollection AddTestDbContext(this IServiceCollection services)
        {
            //services.AddDbContext<TestDbContext>(builder =>
            //{
            //    var connString = "Server=192.168.1.191;Database=try-efcore-db;User=root;Password=Zonekey@2019;Allow User Variables=true;";
            //    var serverVersion = ServerVersion.AutoDetect(connString);
            //    builder.UseMySql(connString, serverVersion);
            //});
            services.AddDbContext<TestDbContext>(builder =>
            {
                var dbFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "try-efcore-db.db");
                var connString = $"Data Source={dbFilePath};";
                builder.UseSqlite(connString);
            });
            return services;
        }
    }
}
