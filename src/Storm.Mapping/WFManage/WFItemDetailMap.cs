using Storm.Domain.Entity.WFManage;
using System.Data.Entity.ModelConfiguration;

namespace Storm.Mapping.WFManage
{
    public class WFItemDetailMap : EntityTypeConfiguration<WFItemDetailEntity>
    {
        public WFItemDetailMap()
        {
            this.ToTable("WF_ItemDetails");
            this.HasKey(t => t.Id);
        }
    }
}
