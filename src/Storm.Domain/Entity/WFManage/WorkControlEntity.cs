using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Domain.Entity.WFManage
{
    public class WorkControlEntity
    {
        public string Id { get; set; }
        public string WorkId { get; set; }
        public string ControlId { get; set; }
        public string FullName { get; set; }
        public int ControlType { get; set; }
        public string TypeName { get; set; }
        public string Value { get; set; }
    }
}
