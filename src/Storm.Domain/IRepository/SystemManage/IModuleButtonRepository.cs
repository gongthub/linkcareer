using Storm.Data;
using Storm.Domain.Entity.SystemManage;
using System.Collections.Generic;

namespace Storm.Domain.IRepository.SystemManage
{
    public interface IModuleButtonRepository : IRepositoryBase<ModuleButtonEntity>
    {
        void SubmitCloneButton(List<ModuleButtonEntity> entitys);
    }
}
