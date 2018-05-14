using Storm.Domain.Entity.WFManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Mapping.WFManage
{
    public class WorkControlMap : EntityTypeConfiguration<WorkControlEntity>
    {
        public WorkControlMap()
        {
            this.ToTable("WF_WorkControls");
            this.HasKey(t => t.Id);
        }
    }
}
