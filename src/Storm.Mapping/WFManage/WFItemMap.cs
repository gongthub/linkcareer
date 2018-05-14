using Storm.Domain.Entity.WFManage;
using System.Data.Entity.ModelConfiguration;

namespace Storm.Mapping.WFManage
{
    public class WFItemMap : EntityTypeConfiguration<WFItemEntity>
    {
        public WFItemMap()
        {
            this.ToTable("WF_Items");
            this.HasKey(t => t.Id);
        }
    }
}
