using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Storm.Domain.Entity.WFManage
{
    public class FlowEntity : IEntity<FlowEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string Id { get; set; }
        public string FormId { get; set; }
        public int FormType { get; set; }
        public string FormUrl { get; set; }
        public string EnCode { get; set; }
        public string FullName { get; set; }
        public int? SortCode { get; set; }
        public bool? DeleteMark { get; set; }
        public bool? EnabledMark { get; set; }
        public string Description { get; set; }
        public DateTime? CreatorTime { get; set; }
        public string CreatorUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }
        public string LastModifyUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string DeleteUserId { get; set; }
        [NotMapped]
        public string Codes { set; get; }
        [NotMapped]
        public int InitNum { set; get; }
        [NotMapped]
        public List<FlowVersionEntity> flowVersions { set; get; }
    }
}
