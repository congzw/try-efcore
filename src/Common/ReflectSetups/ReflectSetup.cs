using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Common
{
    public interface ISetupByReflect<in TRegistry>
    {
        void Setup(TRegistry registry);
    }

    public abstract class ReflectSetupRegistry<TRegistry> where TRegistry : ReflectSetupRegistry<TRegistry>
    {
        public bool ApplyInvoked { get; set; }
        public List<ReflectMethodMeta> Metas { get; set; } = new List<ReflectMethodMeta>();
        public List<SetupLog> Logs { get; set; } = new List<SetupLog>();

        public virtual void Setup(params Assembly[] assemblies)
        {
            var helper = ReflectMethodHelper.Instance;
            if (assemblies == null || assemblies.Length == 0)
            {
                throw new ArgumentException("assemblies至少包含一个元素以上");
            }
            Metas = helper.GetReflectMethodMetas(assemblies).GetGroupMetas<TRegistry>();
        }

        public virtual void Apply()
        {
            if (ApplyInvoked)
            {
                throw new InvalidOperationException("不能重复调用初始化方法");
            }

            ApplyInvoked = true;
            Log("setup apply start");
            foreach (var meta in Metas)
            {
                meta.InvokeMethod(this);
                Log($"    {meta.Order} => {meta.Method}");
            }
            Log("setup apply complete");
        }

        public void Log(object msg)
        {
            Logs.Add(SetupLog.Create(msg));
        }
    }

    public class SetupLog
    {
        public DateTimeOffset CreateAt { get; set; } = DateTimeOffset.Now;
        public object Message { get; set; }

        public static SetupLog Create(object message, DateTimeOffset? createAt = null)
        {
            var applyLog = new SetupLog();
            applyLog.Message = message;
            if (createAt != null)
            {
                applyLog.CreateAt = createAt.Value;
            }
            return applyLog;
        }
    }

    public static class ReflectSetupHelperExtensions
    {
        public static List<ReflectMethodMeta> GetGroupMetas(this IEnumerable<ReflectMethodMeta> metas, string groupName)
        {
            var query = metas.Where(x => x != null);
            if (!string.IsNullOrWhiteSpace(groupName))
            {
                query = query.Where(x => x.Group.MyEquals(groupName));
            }
            return query.OrderBy(x => x.Group).ThenBy(x => x.Order).ToList();
        }
        public static List<ReflectMethodMeta> GetGroupMetas<TGroup>(this IEnumerable<ReflectMethodMeta> metas)
        {
            var groupName = typeof(TGroup).FullName;
            return metas.GetGroupMetas(groupName);
        }

        public static IEnumerable<ReflectMethodMeta> GetReflectMethodMetas(this IReflectMethodHelper helper, params MethodInfo[] methods)
        {
            return GetReflectMethodMetas(helper, (IEnumerable<MethodInfo>)methods);
        }
        public static IEnumerable<ReflectMethodMeta> GetReflectMethodMetas(this IReflectMethodHelper helper, params Type[] theTypes)
        {
            return GetReflectMethodMetas(helper, (IEnumerable<Type>)theTypes);
        }
        public static IEnumerable<ReflectMethodMeta> GetReflectMethodMetas(this IReflectMethodHelper helper, params Assembly[] assemblies)
        {
            return GetReflectMethodMetas(helper, (IEnumerable<Assembly>)assemblies);
        }

        private static IEnumerable<ReflectMethodMeta> GetReflectMethodMetas(IReflectMethodHelper helper, IEnumerable<MethodInfo> methods)
        {
            return methods.Select(helper.GetReflectMethodMeta).Where(x => x != null);
        }
        private static IEnumerable<ReflectMethodMeta> GetReflectMethodMetas(IReflectMethodHelper helper, IEnumerable<Type> theTypes)
        {
            foreach (var theType in theTypes)
            {
                foreach (var method in theType.GetMethods())
                {
                    var meta = helper.GetReflectMethodMeta(method);
                    if (meta != null)
                    {
                        yield return meta;
                    }
                }
            }
        }
        private static IEnumerable<ReflectMethodMeta> GetReflectMethodMetas(IReflectMethodHelper helper, IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                foreach (var theType in assembly.GetTypes())
                {
                    foreach (var method in theType.GetMethods())
                    {
                        var meta = helper.GetReflectMethodMeta(method);
                        if (meta != null)
                        {
                            yield return meta;
                        }
                    }
                }
            }
        }
    }
}
