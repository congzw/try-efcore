using System;

namespace Common.Shared.Contract.Data
{
    /// <summary>
    /// 创建时间
    /// </summary>
    public interface IHaveCreatedAt
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTimeOffset CreatedAt { get; set; }
    }

    /// <summary>
    /// 创建人
    /// </summary>
    public interface IHaveCreatedBy
    {
        /// <summary>
        /// 创建人
        /// </summary>
        string CreatedBy { get; set; }
    }

    /// <summary>
    /// 上次修改时间
    /// </summary>
    public interface IHaveModifiedAt
    {
        /// <summary>
        /// 上次修改时间
        /// </summary>
        DateTimeOffset ModifiedAt { get; set; }
    }

    /// <summary>
    /// 上次修改人
    /// </summary>
    public interface IHaveModifiedBy
    {
        /// <summary>
        /// 上次修改人
        /// </summary>
        string ModifiedBy { get; set; }
    }

    /// <summary>
    /// 包含常用审计字段
    /// </summary>
    public interface IHaveAudit : IHaveCreatedAt, IHaveCreatedBy, IHaveModifiedAt, IHaveModifiedBy
    {
    }

    public class Audit : IHaveAudit
    {
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
    }
}
