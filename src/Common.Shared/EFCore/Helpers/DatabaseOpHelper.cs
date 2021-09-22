using System;
using System.Collections.Generic;
using System.Linq;
using Common.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Common.Shared.EFCore.Helpers
{
    public interface IDatabaseOpHelper
    {
        bool IsDbExist(DbContext dbContext);
        void DeleteDb(DbContext dbContext);
        bool TableExist<TEntity>(DbContext dbContext) where TEntity : class;
        List<string> GetModelTableNames(DbContext dbContext);
    }

    public class DatabaseOpHelper : IDatabaseOpHelper
    {
        public static IDatabaseOpHelper Instance => LazySingleton.Instance.Resolve(() => new DatabaseOpHelper());

        public bool IsDbExist(DbContext dbContext)
        {
            return dbContext.Database.CanConnect();
        }

        public void CreateDb(DbContext dbContext)
        {
            dbContext.Database.EnsureCreated();
        }

        public void DeleteDb(DbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
        }

        //public void ClearDb(DbContext dbContext)
        //{
        //    //For EF Core 2.x
        //    //using (var connection = dbContext.Database.GetDbConnection())
        //    //{
        //    //    connection.Open();
        //    //    using (var command = connection.CreateCommand())
        //    //    {
        //    //        command.CommandText = "TRUNCATE TABLE [TableName]";
        //    //        var result =  command.ExecuteNonQuery();
        //    //    }
        //    //}

        //    ////For EF Core 3.x
        //    //dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE [TableName]");

        //    using (var scope = RootServiceProvider.Instance.Root.CreateScope())
        //    {
        //        //var migrationService = dbContext.Database.GetInfrastructure().GetService<IDbMigrationService>(); => NULL
        //        var migrationService = scope.ServiceProvider.GetService<IMigrationService>();
        //        var dbMigrationContext = migrationService.GetDbMigrationContext();
        //        var emptyDb = dbMigrationContext.MigrationItems.OrderBy(x => x.Id).FirstOrDefault();
        //        if (emptyDb != null)
        //        {
        //            migrationService.ApplyMigrationTo(emptyDb.Id);
        //            //种子数据没有被填充，得到一个空的数据库
        //        }
        //        ////如果使用Migration，则不应采用下面这种做法！
        //        //dbContext.Database.EnsureDeleted();
        //        //dbContext.Database.EnsureCreated();
        //    }
        //}

        private readonly IDictionary<Type, bool> _tableExist = new Dictionary<Type, bool>();
        public bool TableExist<TEntity>(DbContext dbContext) where TEntity : class
        {
            if (_tableExist.TryGetValue(typeof(TEntity), out var exist))
            {
                return exist;
            }

            try
            {
                var count = dbContext.Set<TEntity>().Count();
                _tableExist[typeof(TEntity)] = true;
                return true;
            }
            catch (Exception)
            {
                _tableExist[typeof(TEntity)] = false;
                return false;
            }
        }

        public List<string> GetModelTableNames(DbContext dbContext)
        {
            var tableNames = dbContext.Model.GetEntityTypes()
                .Select(t => t.GetTableName())
                .Distinct()
                .ToList();
            return tableNames;
        }
    }

    public static class DbContextExtensions
    {
        public static bool IsDbExist(this DbContext dbContext)
        {
            return DatabaseOpHelper.Instance.IsDbExist(dbContext);
        }

        public static void DeleteDb(this DbContext dbContext)
        {
            DatabaseOpHelper.Instance.DeleteDb(dbContext);
        }

        public static bool TableExist<TEntity>(this DbContext dbContext) where TEntity : class
        {
            return DatabaseOpHelper.Instance.TableExist<TEntity>(dbContext);
        }

        public static List<string> GetModelTableNames(DbContext dbContext)
        {
            return DatabaseOpHelper.Instance.GetModelTableNames(dbContext);
        }
    }
}
