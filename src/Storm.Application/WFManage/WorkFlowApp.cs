using Storm.Domain.Entity.WFManage;
using Storm.Domain.IRepository.WFManage;
using Storm.Repository.WFManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Application.WFManage
{
    public class WorkFlowApp
    {
        private IWorkFlowRepository service = new WorkFlowRepository();
        private IApprovalProcessRepository approservice = new ApprovalProcessRepository();
        public void Start(string workId)
        {
            service.Start(workId);
        }
        public void Approval(string workId, int status, string desc)
        {
            service.Approval(workId, status, desc);
        }
        public List<ApprovalProcessEntity> GetApproProcessList(string workId)
        {
            List<ApprovalProcessEntity> models = new List<ApprovalProcessEntity>();
            models = approservice.IQueryable(m => m.WorkId == workId && m.DeleteMark != true
                && m.IsEnd != true && m.IsStart != true).OrderByDescending(m => m.CreatorTime).ToList();
            return models;
        }
    }
}
