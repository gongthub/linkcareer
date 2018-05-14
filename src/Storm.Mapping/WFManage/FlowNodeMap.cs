using Storm.Domain.Entity.WFManage;
using System.Data.Entity.ModelConfiguration;

namespace Storm.Mapping.WFManage
{
    public class FlowNodeMap : EntityTypeConfiguration<FlowNodeEntity>
    {
        public FlowNodeMap()
        {
            this.ToTable("WF_FlowNodes");
            this.HasKey(t => t.Id);
        }
    }
}

