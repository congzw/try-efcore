using Common.Shared.Contract.Data;

namespace TryEfCore.Libs.Core.Demos
{
    public class Org : BaseEntity, IHaveRelationCode
    {
        public string Name { get; set; }
        public string ParentId { get; set; }
        public string RelationCode { get; set; }
    }

    public class User : BaseEntity
    {
        public string OrgId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
    }
    
    public class Course : BaseEntity
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string SubjectCode { get; set; }
        public string GradeCode { get; set; }
    }
}
