using System;
using System.Linq;
using Common.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Common.Shared.Contract.Data
{
    public interface IAutoAudit
    {
    }

    public class AutoAuditConfig
    {
        private static readonly AutoAuditConfig DefaultInstance = new AutoAuditConfig();
        public static Func<AutoAuditConfig> GetConfig = () => DefaultInstance;

        public string CreatedAt { get; set; } = "CreatedAt";
        public string CreatedBy { get; set; } = "CreatedBy";
        public string ModifiedAt { get; set; } = "ModifiedAt";
        public string ModifiedBy { get; set; } = "ModifiedBy";
    }

    public class AutoAuditContext
    {
        public string UserId { get; set; } = string.Empty;
        public DateTimeOffset Now { get; set; } = DateHelper.Instance.GetNow();

        public static Func<AutoAuditContext> GetCurrent = () => new AutoAuditContext();
    }

    public static class AutoAuditExtensions
    {
        public static void ApplyAutoAuditOnModelCreating(this ModelBuilder modelBuilder, AutoAuditConfig config = null)
        {
            config ??= AutoAuditConfig.GetConfig();
            // Create shadow properties
            var entityTypes = modelBuilder.Model.GetEntityTypes().ToList();
            var auditEntityTypes = entityTypes.Where(e => typeof(IAutoAudit).IsAssignableFrom(e.ClrType)).ToList();
            foreach (var entityType in auditEntityTypes)
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTimeOffset>(config.CreatedAt);

                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTimeOffset>(config.ModifiedAt);

                modelBuilder.Entity(entityType.ClrType)
                    .Property<string>(config.CreatedBy);

                modelBuilder.Entity(entityType.ClrType)
                    .Property<string>(config.ModifiedBy);
            }
        }

        public static void ApplyAutoAuditOnSaving(this DbContext dbContext)
        {
            var auditCtx = AutoAuditContext.GetCurrent();
            var config = AutoAuditConfig.GetConfig();

            var addOrEditEntities = dbContext.ChangeTracker.Entries<IAutoAudit>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            foreach (var entity in addOrEditEntities)
            {
                entity.Property(config.ModifiedAt).CurrentValue = auditCtx.Now;
                entity.Property(config.ModifiedBy).CurrentValue = auditCtx.UserId;
                if (entity.State == EntityState.Added)
                {
                    entity.Property(config.CreatedAt).CurrentValue = auditCtx.Now;
                    entity.Property(config.CreatedBy).CurrentValue = auditCtx.UserId;
                }
            }
        }

        public static IOrderedQueryable<T> OrderByCreateAt<T>(this IQueryable<T> query)
        {
            var config = AutoAuditConfig.GetConfig();
            return query.OrderBy(x => EF.Property<DateTimeOffset>(x, config.CreatedAt));
        }

        public static IQueryable<AuditEntity<T>> ToAuditEntity<T>(this IQueryable<T> query) where T : IAutoAudit
        {
            var config = AutoAuditConfig.GetConfig();
            var result = query.Select(x => new AuditEntity<T>
            {
                Entity = x,
                CreatedAt = EF.Property<DateTimeOffset>(x, config.CreatedAt),
                CreatedBy = EF.Property<string>(x, config.CreatedBy),
                ModifiedAt = EF.Property<DateTimeOffset>(x, config.ModifiedAt),
                ModifiedBy = EF.Property<string>(x, config.ModifiedBy)
            });
            return result;
        }
    }

    public class AuditEntity<T> : IHaveAudit where T : IAutoAudit
    {
        public T Entity { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
    }
}
