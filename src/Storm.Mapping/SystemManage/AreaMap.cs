using Storm.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Storm.Mapping.SystemManage
{
    public class AreaMap : EntityTypeConfiguration<AreaEntity>
    {
        public AreaMap()
        {
            this.ToTable("Sys_Area");
            this.HasKey(t => t.Id);
        }
    }
}
