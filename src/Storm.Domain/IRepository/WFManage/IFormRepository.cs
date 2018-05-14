using Storm.Data;
using Storm.Domain.Entity.WFManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Domain.IRepository.WFManage
{
    public interface IFormRepository : IRepositoryBase<FormEntity>
    {
        void SaveDesign(FormEntity formEntity, List<FormControlEntity> formControlModels);
        FormControlEntity GetControl(string formId, string controlId);
        FormControlEntity GetControlByWorkId(string workId, string controlId);
    }
}
