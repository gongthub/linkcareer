using Storm.Domain.Entity.WFManage;
using System.Data.Entity.ModelConfiguration;

namespace Storm.Mapping.WFManage
{
    public class FlowMap : EntityTypeConfiguration<FlowEntity>
    {
        public FlowMap()
        {
            this.ToTable("WF_Flows");
            this.HasKey(t => t.Id);
        }
    }
}
