using Storm.Data;
using Storm.Domain.Entity.WFManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Domain.IRepository.WFManage
{
    public interface IFlowVersionRepository : IRepositoryBase<FlowVersionEntity>
    {
        FlowVersionEntity GetNewFlowVersion(string flowId);
        List<FlowLineEntity> GetLines(string flowId);
        FlowLineEntity GetLine(string flowId, string markName);
        List<FlowNodeEntity> GetNodes(string flowId);
        FlowNodeEntity GetNode(string flowId, string markName);
        List<FlowAreaEntity> GetAreas(string flowId);
        FlowAreaEntity GetArea(string flowId, string markName);
        void UpdateLine(FlowLineEntity flowLine);
    }
}
