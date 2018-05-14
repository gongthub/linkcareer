using System;

namespace Storm.Domain.Entity.SystemManage
{
    public class RoleAuthorizeEntity : IEntity<RoleAuthorizeEntity>, ICreationAudited
    {
        public string Id { get; set; }
        public int? ItemType { get; set; }
        public string ItemId { get; set; }
        public int? ObjectType { get; set; }
        public string ObjectId { get; set; }
        public int? SortCode { get; set; }
        public DateTime? CreatorTime { get; set; }
        public string CreatorUserId { get; set; }
    }
}
