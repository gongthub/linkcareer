using Storm.Data;
using Storm.Domain.Entity.WFManage;

namespace Storm.Domain.IRepository.WFManage
{
    public interface IFlowRepository : IRepositoryBase<FlowEntity>
    {
        void SaveDesign(FlowVersionEntity flowVersionEntity);
        string GenGooFlows(string keyValue);
    }
}
