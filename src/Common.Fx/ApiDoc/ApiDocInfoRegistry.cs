using System.Collections.Generic;
using Common.Utilities;

namespace Common.Fx.ApiDoc
{
    public class ApiDocInfoRegistry : ReflectSetupRegistry<ApiDocInfoRegistry>
    {
        #region should used as singleton

        [LazySingleton]
        public static ApiDocInfoRegistry Instance => LazySingleton.Instance.Resolve(() => new ApiDocInfoRegistry());

        #endregion

        public List<ApiDocInfo> ApiDocInfos { get; set; } = new List<ApiDocInfo>();
    }

    public interface IApiDocSetup : ISetupByReflect<ApiDocInfoRegistry>
    {
    }
}
