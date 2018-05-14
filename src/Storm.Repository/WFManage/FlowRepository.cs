using Storm.Data;
using Storm.Domain.Entity.WFManage;
using Storm.Domain.IRepository.WFManage;

namespace Storm.Repository.WFManage
{
    public class FlowRepository : RepositoryBase<FlowEntity>, IFlowRepository
    {
        public void SaveDesign(FlowVersionEntity flowVersionEntity)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                flowVersionEntity.Create();
                if (flowVersionEntity.Nodes != null && flowVersionEntity.Nodes.Count > 0)
                {
                    foreach (var flowNodes in flowVersionEntity.Nodes)
                    {
                        flowNodes.FlowVersionId = flowVersionEntity.Id;
                        db.Insert(flowNodes);
                    }
                }
                if (flowVersionEntity.Lines != null && flowVersionEntity.Lines.Count > 0)
                {
                    foreach (var flowLines in flowVersionEntity.Lines)
                    {
                        flowLines.FlowVersionId = flowVersionEntity.Id;
                        db.Insert(flowLines);
                    }
                }
                if (flowVersionEntity.Areas != null && flowVersionEntity.Areas.Count > 0)
                {
                    foreach (var flowAreas in flowVersionEntity.Areas)
                    {
                        flowAreas.FlowVersionId = flowVersionEntity.Id;
                        db.Insert(flowAreas);
                    }
                }
                db.Insert(flowVersionEntity);
                db.Commit();
            }
        }
        public string GenGooFlows(string keyValue)
        {
            string jsonStr = string.Empty;


            return jsonStr;
        }
        private GooFlowEntity GenGooFlowByFlowId(string keyValue)
        {
            GooFlowEntity gooFlowEntity = new GooFlowEntity();
            FlowEntity flowEntity = FindEntity(keyValue);

            return gooFlowEntity;
        }

    }
}
