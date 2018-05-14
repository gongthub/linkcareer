using Storm.Data;
using Storm.Domain.Entity.WFManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Domain.IRepository.WFManage
{
    public interface IWorkFlowRepository : IRepositoryBase
    {
        void Start(string workId);
        void ApplySuccess(string workId, string desc);
        void ApplyFail(string workId, string desc);
        void Approval(string workId, int status, string desc);
    }
}
