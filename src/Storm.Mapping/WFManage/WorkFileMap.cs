using Storm.Domain.Entity.WFManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Mapping.WFManage
{
    public class WorkFileMap : EntityTypeConfiguration<WorkFileEntity>
    {
        public WorkFileMap()
        {
            this.ToTable("WF_WorkFiles");
            this.HasKey(t => t.Id);
        }
    }
}
