using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Domain.Entity.WFManage
{
    public class FlowLineEntity
    {
        public string Id { set; get; }
        public string FlowVersionId { set; get; }
        public string MarkName { set; get; }
        public string TypeName { set; get; }
        public string Name { set; get; }
        public string FromNode { set; get; }
        public string ToNode { set; get; }
        public bool Marked { set; get; }
        public int PlotType { set; get; }
        public string Plot { set; get; }
        public string SqlPlot { set; get; }
    }
}
