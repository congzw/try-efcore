using Common;
using Common.Fx.ApiDoc;

namespace TryEfCore.Libs.Core.ApiDoc
{
    public class ApiDocForCore : IApiDocSetup
    {
        [ReflectMethod]
        public void Setup(ApiDocInfoRegistry registry)
        {
            var version = "1.0";
            registry.ApiDocInfos.Add(new ApiDocInfo()
            {
                Name = "all-in-one",
                Title = "接口文档",
                Version = version,
                Description = "包含所有接口文档",
                Endpoint = $"/swagger/all-in-one/swagger.json",
                XmlFile = "TryEfCore.Libs.xml"
            });
        }
    }
}