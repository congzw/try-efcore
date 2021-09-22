using System;
using System.Reflection;
using Common.Utilities;

// ReSharper disable once CheckNamespace
namespace Common
{
    public interface IReflectMethodHelper
    {
        ReflectMethodMeta GetReflectMethodMeta(MethodInfo method);
    }

    public class ReflectMethodHelper : IReflectMethodHelper
    {
        #region for di extensions
        
        [LazySingleton]
        public static IReflectMethodHelper Instance => LazySingleton.Instance.Resolve(() => new ReflectMethodHelper());

        #endregion

        public ReflectMethodMeta GetReflectMethodMeta(MethodInfo method)
        {
            return ReflectMethodMeta.TryCreate(method);
        }
    }

    public class ReflectMethodMeta
    {
        public string Group { get; set; }
        public int Order { get; set; }
        public string Method { get; set; }

        #region not serializable

        private MethodInfo _methodInfo;

        public MethodInfo GetMethodInfo()
        {
            return _methodInfo;
        }

        public void SetMethodInfo(MethodInfo value)
        {
            _methodInfo = value;
            Method = $"{GetMethodDescription(value)}";
        }


        private static string GetMethodDescription(MethodBase method, object instance = null)
        {
            return instance != null
                ? $"{method.DeclaringType?.FullName}.{method.Name}[@]{instance.GetHashCode()}"
                : $"{method.DeclaringType?.FullName}.{method.Name}";
        }

        #endregion

        public static ReflectMethodMeta TryCreate(MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));

            var theAttr = method.GetCustomAttribute<ReflectMethodAttribute>();
            if (theAttr == null)
            {
                return null;
            }

            var meta = new ReflectMethodMeta();
            var theGroup = theAttr.Group;
            if (string.IsNullOrWhiteSpace(theGroup))
            {
                var parameters = method.GetParameters();
                if (parameters.Length > 0)
                {
                    var registryParam = parameters[0];
                    var registryType = registryParam.ParameterType;
                    theGroup = registryType.FullName;
                }
            }

            meta.Group = theGroup;
            meta.Order = theAttr.Order;
            meta.SetMethodInfo(method);
            return meta;
        }

        public void InvokeMethod(params object[] parameters)
        {
            var method = this.GetMethodInfo();
            if (method.IsStatic)
            {
                method.Invoke(null, parameters);
                //method.Invoke(null, new[] { registry });
            }
            else
            {
                var declaringType = method.DeclaringType;
                if (declaringType == null)
                {
                    throw new InvalidOperationException($"没有找到声明方法{Method}的Class Type!");
                }
                var instance = Activator.CreateInstance(declaringType);
                //method.Invoke(instance, new[] { registry });
                method.Invoke(instance, parameters);
            }
        }
    }

    /// <summary>
    /// 标识用于反射发现和调用的方法，适用于没有第三方依赖的场景
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ReflectMethodAttribute : Attribute
    {
        public string Group { get; set; } = string.Empty;
        public int Order { get; set; } = 0;
    }
}
