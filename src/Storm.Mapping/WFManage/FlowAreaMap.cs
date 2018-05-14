using Storm.Domain.Entity.WFManage;
using System.Data.Entity.ModelConfiguration;

namespace Storm.Mapping.WFManage
{
    public class FlowAreaMap : EntityTypeConfiguration<FlowAreaEntity>
    {
        public FlowAreaMap()
        {
            this.ToTable("WF_FlowAreas");
            this.HasKey(t => t.Id);
        }
    }
}