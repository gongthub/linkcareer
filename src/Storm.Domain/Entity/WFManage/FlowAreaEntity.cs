using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Domain.Entity.WFManage
{
    public class FlowAreaEntity
    {
        public string Id { set; get; }
        public string FlowVersionId { set; get; }
        public string MarkName { set; get; }
        public string Name { set; get; }
        public bool Marked { set; get; }
        public int Left { set; get; }
        public int Top { set; get; }
        public int Width { set; get; }
        public int Height { set; get; }
    }
}
