using Storm.Data;
using Storm.Domain.Entity.WFManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Domain.IRepository.WFManage
{
    public interface IWorkRepository : IRepositoryBase<WorkEntity>
    {
        void AddForm(WorkEntity workEntity, List<WorkControlEntity> controls, List<WorkFileEntity> files);
        void UpdateForm(WorkEntity workEntity, List<WorkControlEntity> controls, List<WorkFileEntity> files, List<string> RemoveFileIds);
        List<WorkControlEntity> GetWorkControls(string workIds);
        List<WorkFileEntity> GetWorkFiles(string workIds);
        List<MyPendingWorkEntity> GetMyPendingList(string keyword = "");
        
    }
}
