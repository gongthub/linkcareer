using Storm.Code;
using Storm.Domain.Entity.WFManage;
using Storm.Domain.IRepository.WFManage;
using Storm.Repository.WFManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Storm.Application.WFManage
{
    public class WorkApp
    {
        private static string WFFILEPATHS = Configs.GetValue("WFFilePaths");
        private IWorkRepository service = new WorkRepository();
        private FlowApp flowApp = new FlowApp();
        private FormApp formApp = new FormApp();
        private WorkFlowApp workFlowApp = new WorkFlowApp();
        public List<WorkEntity> GetAllList(string keyword = "")
        {
            var expression = ExtLinq.True<WorkEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.FullName.Contains(keyword));
            }
            List<WorkEntity> models = service.IQueryable(expression).OrderByDescending(t => t.CreatorTime).ToList();
            if (models != null && models.Count > 0)
            {
                models.ForEach(delegate(WorkEntity model)
                {
                    string desc = Code.EnumHelp.enumHelp.GetDescription(typeof(WorkStatus), model.FlowStatus);
                    model.FlowStatusName = desc;
                });
            }
            return models;
        }
        public List<WorkEntity> GetList(string keyword = "")
        {
            var expression = ExtLinq.True<WorkEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.FullName.Contains(keyword));
            }
            expression = expression.And(t => t.DeleteMark != true);
            List<WorkEntity> models = service.IQueryable(expression).OrderByDescending(t => t.CreatorTime).ToList();
            if (models != null && models.Count > 0)
            {
                models.ForEach(delegate(WorkEntity model)
                {
                    string desc = Code.EnumHelp.enumHelp.GetDescription(typeof(WorkStatus), model.FlowStatus);
                    model.FlowStatusName = desc;
                });
            }
            return models;
        }
        public List<WorkEntity> GetEnableList(string keyword = "")
        {
            var expression = ExtLinq.True<WorkEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.FullName.Contains(keyword));
            }
            expression = expression.And(t => t.DeleteMark != true && t.EnabledMark == true);
            List<WorkEntity> models = service.IQueryable(expression).OrderByDescending(t => t.CreatorTime).ToList();
            if (models != null && models.Count > 0)
            {
                models.ForEach(delegate(WorkEntity model)
                {
                    string desc = Code.EnumHelp.enumHelp.GetDescription(typeof(WorkStatus), model.FlowStatus);
                    model.FlowStatusName = desc;
                });
            }
            return models;
        }
        public List<WorkEntity> GetMyWorkList(string keyword = "")
        {
            var expression = ExtLinq.True<WorkEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.FullName.Contains(keyword));
            }
            var loguser = OperatorProvider.Provider.GetCurrent();
            if (loguser != null)
            {
                expression = expression.And(t => t.ApplyUserId == loguser.UserId);
            }
            expression = expression.And(t => t.DeleteMark != true);
            List<WorkEntity> models = service.IQueryable(expression).OrderByDescending(t => t.CreatorTime).ToList();
            if (models != null && models.Count > 0)
            {
                models.ForEach(delegate(WorkEntity model)
                {
                    string desc = Code.EnumHelp.enumHelp.GetDescription(typeof(WorkStatus), model.FlowStatus);
                    model.FlowStatusName = desc;
                });
            }
            return models;
        }
        public List<MyPendingWorkEntity> GetMyPendingList(string keyword = "")
        {
            List<MyPendingWorkEntity> models = service.GetMyPendingList(keyword);
            if (models != null && models.Count > 0)
            {
                models.ForEach(delegate(MyPendingWorkEntity model)
                {
                    string desc = Code.EnumHelp.enumHelp.GetDescription(typeof(WorkStatus), model.FlowStatus);
                    model.FlowStatusName = desc;
                });
            }
            return models;
        }
        public WorkEntity GetForm(string keyValue)
        {
            WorkEntity model = service.FindEntity(keyValue);
            return model;
        }
        public void DeleteForm(string keyValue)
        {
            service.Delete(t => t.Id == keyValue);
        }

        public List<WorkControlEntity> GetWorkControls(string workIds)
        {
            List<WorkControlEntity> models = new List<WorkControlEntity>();
            models = service.GetWorkControls(workIds);
            return models;
        }
        public List<WorkFileEntity> GetWorkFiles(string workIds)
        {
            List<WorkFileEntity> models = new List<WorkFileEntity>();
            models = service.GetWorkFiles(workIds);
            return models;
        }

        public void AddForm(string flowId, int status, string contents, List<WorkControlEntity> controls, List<WorkFileEntity> files)
        {
            WorkEntity workEntity = new WorkEntity();
            if (status == (int)WorkStatus.Save || status == (int)WorkStatus.Applying)
            {
                if (flowId != null)
                {
                    FlowEntity flowentity = flowApp.GetForm(flowId);
                    if (flowentity != null && !string.IsNullOrEmpty(flowentity.Id))
                    {
                        FormEntity formEntity = formApp.GetForm(flowentity.FormId);
                        FlowVersionEntity flowVersionEntity = flowApp.GetDesign(flowId);
                        if (flowentity != null && !string.IsNullOrEmpty(flowentity.Id)
                            && formEntity != null && !string.IsNullOrEmpty(formEntity.Id)
                            && flowVersionEntity != null && !string.IsNullOrEmpty(flowVersionEntity.Id))
                        {
                            workEntity.Create();
                            workEntity.FullName = flowentity.FullName;
                            workEntity.FlowVersionId = flowVersionEntity.Id;
                            workEntity.FlowStatus = status;
                            workEntity.Codes = formEntity.Codes;
                            workEntity.Contents = contents;
                            var loguser = OperatorProvider.Provider.GetCurrent();
                            if (loguser != null)
                            {
                                workEntity.ApplyUserId = loguser.UserId;
                            }
                            service.AddForm(workEntity, controls, files);
                            if (status == (int)WorkStatus.Applying)
                            {
                                try
                                {
                                    workFlowApp.Start(workEntity.Id);
                                }
                                catch (Exception ex)
                                {
                                    workEntity.FlowStatus = (int)WorkStatus.Save;
                                    service.Update(workEntity);
                                    throw ex;
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("操作失败！");
                        }
                    }
                    else
                    {
                        throw new Exception("操作失败！");
                    }
                }
                else
                {
                    throw new Exception("操作失败！");
                }
            }
            else
            {
                throw new Exception("操作失败，提交状态无效！");
            }
        }
        public void UpdateForm(string workId, int status, string contents, List<WorkControlEntity> controls, List<WorkFileEntity> files, List<string> RemoveFileIds)
        {
            WorkEntity workEntity = new WorkEntity();
            if (status == (int)WorkStatus.Save || status == (int)WorkStatus.Applying)
            {
                if (workId != null)
                {
                    workEntity = GetForm(workId);
                    if (workEntity != null && !string.IsNullOrEmpty(workEntity.Id))
                    {
                        if (workEntity.FlowStatus != (int)WorkStatus.Save)
                        {
                            throw new Exception("该申请已申请，不能修改！");
                        }
                        workEntity.Modify(workId);
                        workEntity.FlowStatus = status;
                        workEntity.Contents = contents;
                        service.UpdateForm(workEntity, controls, files, RemoveFileIds);
                        if (status == (int)WorkStatus.Applying)
                        {
                            try
                            {
                                workFlowApp.Start(workEntity.Id);
                            }
                            catch (Exception ex)
                            {
                                workEntity.FlowStatus = (int)WorkStatus.Save;
                                service.Update(workEntity);
                                throw ex;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("操作失败！");
                    }
                }
                else
                {
                    throw new Exception("操作失败！");
                }
            }
            else
            {
                throw new Exception("操作失败，提交状态无效！");
            }
        }

        public List<WorkFileEntity> UploadFiles(HttpFileCollectionBase Files)
        {
            try
            {
                List<WorkFileEntity> models = new List<WorkFileEntity>();
                if (Files != null && Files.Count > 0)
                {
                    string basePath = Code.FileHelper.BasePath;
                    string upPaths = GetFilePathByDate();
                    for (int i = 0; i < Files.Count; i++)
                    {
                        string keyIds = Files.Keys[i];
                        string controlIds = string.Empty;
                        if (!string.IsNullOrEmpty(keyIds))
                        {
                            string[] strs = keyIds.Split('|');
                            if (strs != null && strs.Count() > 0)
                            {
                                controlIds = strs[0];
                            }
                        }
                        var file = Files[i];
                        string fileName = Path.GetFileName(Files[i].FileName);// 原始文件名称
                        string fileExtension = Path.GetExtension(fileName); // 文件扩展名
                        string saveName = fileName + fileExtension; // 保存文件名称
                        string fileDirPaths = upPaths + "/" + controlIds + "/";
                        string filePaths = fileDirPaths + saveName;
                        string fullPaths = basePath + fileDirPaths;
                        Code.FileHelper.CreateDirectory(fullPaths);

                        Files[i].SaveAs(fullPaths + saveName);
                        WorkFileEntity model = new WorkFileEntity();
                        model.ControlId = controlIds;
                        model.FullName = fileName;
                        model.FullName = fileName;
                        model.Paths = filePaths;
                        models.Add(model);
                    }
                }
                return models;
            }
            catch
            {
                throw new Exception("上传文件失败");
            }
        }
        private string GetFilePathByDate()
        {
            string strPath = string.Empty;
            string dates = DateTime.Now.ToString("yyyyMMdd");
            strPath = WFFILEPATHS + dates + "/";
            return strPath;
        }

        public string GetFormDesign(string flowId)
        {
            string strContents = string.Empty;
            FlowEntity flowEntity = flowApp.GetForm(flowId);
            if (flowEntity != null && !string.IsNullOrEmpty(flowEntity.Id) && !string.IsNullOrEmpty(flowEntity.FormId))
            {
                FormEntity formEntity = formApp.GetForm(flowEntity.FormId);
                strContents = formEntity.Codes;
            }
            return strContents;
        }

    }
}
