using Storm.Domain.Entity.WFManage;
using System.Data.Entity.ModelConfiguration;

namespace Storm.Mapping.WFManage
{
    public class FlowVersionMap : EntityTypeConfiguration<FlowVersionEntity>
    {
        public FlowVersionMap()
        {
            this.ToTable("WF_FlowVersions");
            this.HasKey(t => t.Id);
        }
    }
}