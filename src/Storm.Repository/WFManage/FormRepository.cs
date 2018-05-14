using Storm.Data;
using Storm.Domain.Entity.WFManage;
using Storm.Domain.IRepository.WFManage;
using System;
using System.Collections.Generic;

namespace Storm.Repository.WFManage
{
    public class FormRepository : RepositoryBase<FormEntity>, IFormRepository
    {
        public void SaveDesign(FormEntity formEntity, List<FormControlEntity> formControlModels)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                formEntity.Modify(formEntity.Id);
                db.Delete<FormControlEntity>(m => m.FormId == formEntity.Id);
                if (formControlModels != null && formControlModels.Count > 0)
                {
                    foreach (var item in formControlModels)
                    {
                        item.Id = Guid.NewGuid().ToString();
                        item.FormId = formEntity.Id;
                        db.Insert(item);
                    }
                }
                db.Update(formEntity);
                db.Commit();
            }
        }
        public FormControlEntity GetControl(string formId, string controlId)
        {
            using (var db = new RepositoryBase())
            {
                return db.FindEntity<FormControlEntity>(m => m.FormId == formId && m.ControlId == controlId);
            }
        }


        public FormControlEntity GetControlByWorkId(string workId, string controlId)
        {
            FormControlEntity model = new FormControlEntity();

            using (var db = new RepositoryBase())
            {
                WorkEntity workEntity = db.FindEntity<WorkEntity>(m => m.Id == workId);
                if (workEntity != null && !string.IsNullOrEmpty(workEntity.Id))
                {
                    FlowVersionEntity flowVersionEntity = db.FindEntity<FlowVersionEntity>(m => m.Id == workEntity.FlowVersionId);
                    if (flowVersionEntity != null && !string.IsNullOrEmpty(flowVersionEntity.Id))
                    {
                        FlowEntity flowEntity = db.FindEntity<FlowEntity>(m => m.Id == flowVersionEntity.FlowId);
                        if (flowEntity != null && !string.IsNullOrEmpty(flowEntity.Id))
                        {
                            FormEntity formEntity = db.FindEntity<FormEntity>(m => m.Id == flowEntity.FormId);
                            if (formEntity != null && !string.IsNullOrEmpty(formEntity.Id))
                            {
                                model = db.FindEntity<FormControlEntity>(m => m.FormId == formEntity.Id && m.ControlId == controlId);
                            }
                        }
                    }
                }
            }
            return model;
        }
    }
}
