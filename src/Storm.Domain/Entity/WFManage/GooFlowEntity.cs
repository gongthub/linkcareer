using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Domain.Entity.WFManage
{
    public class GooFlowEntity
    {
        public string title { set; get; }
        public List<GooFlowNodeEntity> nodes { set; get; }
        public List<GooFlowLineEntity> lines { set; get; }
        public List<GooFlowAreaEntity> areas { set; get; }
        public int initNum { set; get; }
    }

    public class GooFlowNodeEntity
    {
        public string name { set; get; }
        public string type { set; get; }
        public int left { set; get; }
        public int top { set; get; }
        public int width { set; get; }
        public int height { set; get; }
        public bool alt { set; get; }
        public bool marked { set; get; }
    }
    public class GooFlowLineEntity
    {
        public string name { set; get; }
        public string type { set; get; }
        public int M { set; get; }
        public string from { set; get; }
        public string to { set; get; }
        public bool marked { set; get; }
    }
    public class GooFlowAreaEntity
    {
        public string name { set; get; }
        public string color { set; get; }
        public int left { set; get; }
        public int top { set; get; }
        public int width { set; get; }
        public int height { set; get; }
        public bool alt { set; get; }
        public bool marked { set; get; }
    }
}
