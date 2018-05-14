using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Domain.Entity.WFManage
{
    public class ApprovalProcessEntity : IEntity<ApprovalProcessEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string Id { get; set; }
        public string WorkId { get; set; }
        public string LastProcessId { get; set; }
        public string ApprovalUserId { get; set; }
        public string ApprovalUserName { get; set; }
        public int ApprovalStatus { get; set; }
        public string NodeId { get; set; }
        public string NodeName { get; set; }
        public string LastLineId { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
        public bool? DeleteMark { get; set; }
        public bool? EnabledMark { get; set; }
        public string Description { get; set; }
        public DateTime? CreatorTime { get; set; }
        public string CreatorUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }
        public string LastModifyUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string DeleteUserId { get; set; }
    }
}
