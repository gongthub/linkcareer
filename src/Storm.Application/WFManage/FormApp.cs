using Storm.Code;
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
    public class FormApp
    {
        private IFormRepository service = new FormRepository();
        private FlowApp flowApp = new FlowApp();

        public List<FormEntity> GetAllList(string keyword = "")
        {
            var expression = ExtLinq.True<FormEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.FullName.Contains(keyword));
                expression = expression.Or(t => t.EnCode.Contains(keyword));
            }
            return service.IQueryable(expression).OrderBy(t => t.SortCode).ToList();
        }
        public List<FormEntity> GetList(string keyword = "")
        {
            var expression = ExtLinq.True<FormEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.FullName.Contains(keyword));
                expression = expression.Or(t => t.EnCode.Contains(keyword));
            }
            expression = expression.And(t => t.DeleteMark != true);
            return service.IQueryable(expression).OrderBy(t => t.SortCode).ToList();
        }
        public List<FormEntity> GetEnableList(string keyword = "")
        {
            var expression = ExtLinq.True<FormEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.FullName.Contains(keyword));
                expression = expression.Or(t => t.EnCode.Contains(keyword));
            }
            expression = expression.And(t => t.DeleteMark != true && t.EnabledMark == true);
            return service.IQueryable(expression).OrderBy(t => t.SortCode).ToList();
        }
        public FormEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.Id == keyValue);
        }
        public void EnbaledForm(string keyValue)
        {
            FormEntity formEntity = GetForm(keyValue);
            if (formEntity != null && !string.IsNullOrEmpty(formEntity.Id))
            {
                formEntity.Modify(keyValue);
                formEntity.EnabledMark = true;
                service.Update(formEntity);
            }
            else
            {
                throw new Exception("获取数据异常！");
            }
        }
        public void DisabledForm(string keyValue)
        {
            FormEntity formEntity = GetForm(keyValue);
            if (formEntity != null && !string.IsNullOrEmpty(formEntity.Id))
            {
                formEntity.Modify(keyValue);
                formEntity.EnabledMark = false;
                service.Update(formEntity);
            }
            else
            {
                throw new Exception("获取数据异常！");
            }
        }
        public void SubmitForm(FormEntity formEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                formEntity.Modify(keyValue);
                service.Update(formEntity);
            }
            else
            {
                formEntity.EnabledMark = true;
                formEntity.Create();
                service.Insert(formEntity);
            }
        }
        public void SaveDesign(string keyValue, string codes, List<FormControlEntity> formControlModels)
        {
            FormEntity formEntity = GetForm(keyValue);
            if (formEntity != null && !string.IsNullOrEmpty(formEntity.Id))
            {
                formEntity.Codes = codes;
                service.SaveDesign(formEntity, formControlModels);
            }
            else
            {
                throw new Exception("获取数据异常！");
            }
        }

        /// <summary>
        /// 获取当前流程申请时默认值
        /// </summary>
        /// <param name="flowId"></param>
        public List<EnumModel> GetCommonDefaultPrograms(string flowId)
        {
            List<EnumModel> models = EnumHelp.enumHelp.EnumToList(typeof(FormDefaultProgram));
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            foreach (var model in models)
            {
                switch (model.Value)
                {
                    case (int)FormDefaultProgram.ApplyUserID:
                        model.Desc = LoginInfo.UserId;
                        break;
                    case (int)FormDefaultProgram.ApplyUserName:
                        model.Desc = LoginInfo.UserName;
                        break;
                    case (int)FormDefaultProgram.ApplyUserDeptID:
                        model.Desc = LoginInfo.DepartmentId;
                        break;
                    case (int)FormDefaultProgram.ApplyUserDeptName:
                        model.Desc = LoginInfo.DepartmentName;
                        break;
                    case (int)FormDefaultProgram.ShortDate:
                        model.Desc = DateTime.Now.ToString("yyyy-MM-dd");
                        break;
                    case (int)FormDefaultProgram.LongDate:
                        model.Desc = DateTime.Now.ToString("yyyy年MM月dd日");
                        break;
                    case (int)FormDefaultProgram.ShortDateTime:
                        model.Desc = DateTime.Now.ToString("HH:mm");
                        break;
                    case (int)FormDefaultProgram.LongDateTime:
                        model.Desc = DateTime.Now.ToString("HH时mm分");
                        break;
                    case (int)FormDefaultProgram.ShortDateAndDateTime:
                        model.Desc = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                        break;
                    case (int)FormDefaultProgram.LongDateAndDateTime:
                        model.Desc = DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分");
                        break;
                    case (int)FormDefaultProgram.FlowName:
                        FlowEntity flowEntity = flowApp.GetForm(flowId);
                        if (flowEntity != null && !string.IsNullOrEmpty(flowEntity.Id))
                        {
                            model.Desc = flowEntity.FullName;
                        }
                        break;
                    default:
                        model.Desc = string.Empty;
                        break;
                }
            }
            return models;
        }
        /// <summary>
        /// 获取当前流程申请时自定义默认值
        /// </summary>
        /// <param name="flowId"></param>
        public string GetCommonCustomDefaultValuesJson(string flowId, string controlId)
        {
            string strValues = string.Empty;

            FlowEntity flowEntity = flowApp.GetForm(flowId);
            if (flowEntity != null && !string.IsNullOrEmpty(flowEntity.Id) && !string.IsNullOrEmpty(flowEntity.FormId))
            {
                FormControlEntity formControl = service.GetControl(flowEntity.FormId, controlId);
                if (formControl != null && !string.IsNullOrEmpty(formControl.Id))
                {
                    strValues = formControl.DefaultValue;
                }
            }
            return strValues;
        }
        /// <summary>
        /// 获取当前流程申请时自定义默认值
        /// </summary>
        /// <param name="flowId"></param>
        public string GetCommonCustomDefaultTypeJson(string flowId, string controlId)
        {
            string strTypes = string.Empty;

            FlowEntity flowEntity = flowApp.GetForm(flowId);
            if (flowEntity != null && !string.IsNullOrEmpty(flowEntity.Id) && !string.IsNullOrEmpty(flowEntity.FormId))
            {
                FormControlEntity formControl = service.GetControl(flowEntity.FormId, controlId);
                if (formControl != null && !string.IsNullOrEmpty(formControl.Id))
                {
                    strTypes = formControl.DefaultType;
                }
            }
            return strTypes;
        }
        /// <summary>
        /// 获取当前流程申请时自定义默认值
        /// </summary>
        /// <param name="flowId"></param>
        public string GetCommonCustomDefaultTypeByWorkIdJson(string workId, string controlId)
        {
            string strTypes = string.Empty;

            FormControlEntity formControl = service.GetControlByWorkId(workId, controlId);
            if (formControl != null && !string.IsNullOrEmpty(formControl.Id))
            {
                strTypes = formControl.DefaultType;
            }
            return strTypes;
        }
    }
}
