using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Domain.Entity.WFManage
{
    public class FlowNodeEntity
    {
        public string Id { set; get; }
        public string FlowVersionId { set; get; }
        public string TypeName { set; get; }
        public string MarkName { set; get; }
        public string Name { set; get; }
        public bool Marked { set; get; }
        public int Left { set; get; }
        public int Top { set; get; }
        public int Width { set; get; }
        public int Height { set; get; }
        public int StepType { set; get; }
        public int RejectType { set; get; }
        public int ReviewerType { set; get; }
        public string ReviewerOrg { set; get; }
        public string ReviewerUser { set; get; }
        public int MessageType { set; get; }
        public bool IsStartNode { set; get; }
        public bool IsEndNode { set; get; }
    }
}
