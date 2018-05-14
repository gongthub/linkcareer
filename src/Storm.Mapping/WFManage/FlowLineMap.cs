using Storm.Domain.Entity.WFManage;
using System.Data.Entity.ModelConfiguration;

namespace Storm.Mapping.WFManage
{
    public class FlowLineMap : EntityTypeConfiguration<FlowLineEntity>
    {
        public FlowLineMap()
        {
            this.ToTable("WF_FlowLines");
            this.HasKey(t => t.Id);
        }
    }
}