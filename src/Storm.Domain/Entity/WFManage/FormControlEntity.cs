using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Domain.Entity.WFManage
{
    public class FormControlEntity
    {
        public string Id { set; get; }
        public string FormId { set; get; }
        public string ControlId { set; get; }
        public string FullName { set; get; }
        public int ControlType { set; get; }
        public string TypeName { set; get; }
        public string DefaultType { set; get; }
        public string DefaultValue { set; get; }
    }
}
