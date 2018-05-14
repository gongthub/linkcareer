using Storm.Domain.Entity.CertificateManage;
using System.Data.Entity.ModelConfiguration;

namespace Storm.Mapping.CertificateManage
{
    public class CertificateMap : EntityTypeConfiguration<CertificateEntity>
    {
        public CertificateMap()
        {
            this.ToTable("C_Certificates");
            this.HasKey(t => t.Id);
        }
    }
}
