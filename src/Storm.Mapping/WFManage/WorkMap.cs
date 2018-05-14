using Storm.Domain.Entity.WFManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Mapping.WFManage
{
    public class WorkMap : EntityTypeConfiguration<WorkEntity>
    {
        public WorkMap()
        {
            this.ToTable("WF_Works");
            this.HasKey(t => t.Id);
        }
    }
}
