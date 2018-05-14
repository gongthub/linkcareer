using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Domain.Entity.CertificateManage
{
    public class CertificateEntity : IEntity<CertificateEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string Id { get; set; }
        public int? SortCode { get; set; }
        public string FullName { get; set; }
        public int? Gender { get; set; }
        public string IdCard { get; set; }
        public string ProjectName { get; set; }
        public string Number { get; set; }
        public DateTime? CertificationTime { get; set; }
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
