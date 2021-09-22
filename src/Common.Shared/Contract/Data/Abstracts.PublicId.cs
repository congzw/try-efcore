using System;
using Common.Utilities;

namespace Common.Shared.Contract.Data
{
    /// <summary>
    /// 公开Id
    /// 可能来自外部导入数据，它的Id源可能是不同的类型：int,guid,...
    /// 也可能内部自动创建，字符串类型
    /// </summary>
    public interface IHavePublicId
    {
        /// <summary>
        /// 公开Id
        /// </summary>
        public string PublicId { get; set; }
    }

    #region extensions
    
    /// <summary>
    /// 自动初始化一个string类型的公开ID
    /// </summary>
    public interface IAutoInitPublicId
    {
        /// <summary>
        /// 自动初始化一个string类型的公开ID
        /// </summary>
        /// <returns></returns>
        string InitPublicIdIf();
    }

    public static class AutoInitPublicIdExtensions
    {
        public static string InitPublicIdIf(this IEntity entity)
        {
            return AutoInitPublicIdHelper.Instance.InitPublicIdIf(entity);
        }
    }

    /// <summary>
    /// 支持定制替换的方法逻辑类，默认逻辑：IAutoInitPublicId > IHavePublicId > IEntity.Id
    /// </summary>
    public class AutoInitPublicIdHelper
    {
        public Func<object, string> InitPublicIdIf { get; set; } = DefaultInitPublicIdIf;
        private static string DefaultInitPublicIdIf(object entity)
        {
            if (entity is IAutoInitPublicId autoInitPublicId)
            {
                return autoInitPublicId.InitPublicIdIf();
            }

            if (entity is IHavePublicId havePublicId)
            {
                if (string.IsNullOrWhiteSpace(havePublicId.PublicId))
                {
                    havePublicId.PublicId = IdGenerator.GetNextId();
                }
                return havePublicId.PublicId;
            }

            if (entity is IEntity<string> theEntity)
            {
                if (string.IsNullOrWhiteSpace(theEntity.Id))
                {
                    theEntity.Id = IdGenerator.GetNextId();
                }
                return theEntity.Id;
            }
            return null;
        }


        public static AutoInitPublicIdHelper Instance = new AutoInitPublicIdHelper();
    }

    #endregion
}
