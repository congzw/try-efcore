namespace Common.Shared.Contract.Data
{
    /// <summary>
    /// 树的关系码，用于简化遍历树节点的查询
    /// </summary>
    public interface IHaveRelationCode
    {
        string RelationCode { get; set; }
    }
}
