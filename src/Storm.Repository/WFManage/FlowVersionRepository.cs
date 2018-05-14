using Storm.Data;
using Storm.Domain.Entity.WFManage;
using Storm.Domain.IRepository.WFManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Repository.WFManage
{
    public class FlowVersionRepository : RepositoryBase<FlowVersionEntity>, IFlowVersionRepository
    {
        public FlowVersionEntity GetNewFlowVersion(string flowId)
        {
            FlowVersionEntity entity = new FlowVersionEntity();
            entity = IQueryable(m => m.DeleteMark != true && m.FlowId == flowId).OrderByDescending(m => m.CreatorTime).FirstOrDefault();
            return entity;
        }
        public List<FlowLineEntity> GetLines(string flowId)
        {
            List<FlowLineEntity> flowLineModels = new List<FlowLineEntity>();
            FlowVersionEntity flowVersionModel = GetNewFlowVersion(flowId);
            if (flowVersionModel != null && !string.IsNullOrEmpty(flowVersionModel.Id))
            {
                using (var db = new RepositoryBase())
                {
                    flowLineModels = db.IQueryable<FlowLineEntity>(m => m.FlowVersionId == flowVersionModel.Id).ToList();
                }
            }
            return flowLineModels;
        }

        public FlowLineEntity GetLine(string flowId, string markName)
        {
            FlowLineEntity flowLineModel = new FlowLineEntity();
            FlowVersionEntity flowVersionModel = GetNewFlowVersion(flowId);
            if (flowVersionModel != null && !string.IsNullOrEmpty(flowVersionModel.Id))
            {
                using (var db = new RepositoryBase())
                {
                    flowLineModel = db.IQueryable<FlowLineEntity>(m => m.FlowVersionId == flowVersionModel.Id && m.MarkName == markName).FirstOrDefault();
                }
            }
            return flowLineModel;
        }

        public List<FlowNodeEntity> GetNodes(string flowId)
        {
            List<FlowNodeEntity> flowNodeModels = new List<FlowNodeEntity>();
            FlowVersionEntity flowVersionModel = GetNewFlowVersion(flowId);
            if (flowVersionModel != null && !string.IsNullOrEmpty(flowVersionModel.Id))
            {
                using (var db = new RepositoryBase())
                {
                    flowNodeModels = db.IQueryable<FlowNodeEntity>(m => m.FlowVersionId == flowVersionModel.Id).ToList();
                }
            }
            return flowNodeModels;
        }

        public FlowNodeEntity GetNode(string flowId, string markName)
        {
            FlowNodeEntity flowNodeModel = new FlowNodeEntity();
            FlowVersionEntity flowVersionModel = GetNewFlowVersion(flowId);
            if (flowVersionModel != null && !string.IsNullOrEmpty(flowVersionModel.Id))
            {
                using (var db = new RepositoryBase())
                {
                    flowNodeModel = db.IQueryable<FlowNodeEntity>(m => m.FlowVersionId == flowVersionModel.Id && m.MarkName == markName).FirstOrDefault();
                }
            }
            return flowNodeModel;
        }

        public List<FlowAreaEntity> GetAreas(string flowId)
        {
            List<FlowAreaEntity> flowAreaModels = new List<FlowAreaEntity>();
            FlowVersionEntity flowVersionModel = GetNewFlowVersion(flowId);
            if (flowVersionModel != null && !string.IsNullOrEmpty(flowVersionModel.Id))
            {
                using (var db = new RepositoryBase())
                {
                    flowAreaModels = db.IQueryable<FlowAreaEntity>(m => m.FlowVersionId == flowVersionModel.Id).ToList();
                }
            }
            return flowAreaModels;
        }

        public FlowAreaEntity GetArea(string flowId, string markName)
        {
            FlowAreaEntity flowAreaModel = new FlowAreaEntity();
            FlowVersionEntity flowVersionModel = GetNewFlowVersion(flowId);
            if (flowVersionModel != null && !string.IsNullOrEmpty(flowVersionModel.Id))
            {
                using (var db = new RepositoryBase())
                {
                    flowAreaModel = db.IQueryable<FlowAreaEntity>(m => m.FlowVersionId == flowVersionModel.Id && m.MarkName == markName).FirstOrDefault();
                }
            }
            return flowAreaModel;
        }

        public void UpdateLine(FlowLineEntity flowLine)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Update(flowLine);
                db.Commit();
            }
        }
    }
}
