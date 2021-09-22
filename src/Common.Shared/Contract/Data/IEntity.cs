namespace Common.Shared.Contract.Data
{
    /// <summary>
    /// 实体类的接口
    /// </summary>
    public interface IEntity
    {
    }

    /// <summary>
    /// 实体类（主键名Id）
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IEntity<TPrimaryKey> : IEntity
    {
        /// <summary>
        /// Entity Primary Key
        /// </summary>
        TPrimaryKey Id { get; set; }
    }
}