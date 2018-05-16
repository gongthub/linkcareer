using Storm.Application.SystemManage;
using Storm.Code;
using Storm.Domain.Entity.CertificateManage;
using Storm.Domain.Entity.SystemManage;
using Storm.Domain.IRepository.CertificateManage;
using Storm.Repository.CertificateManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Storm.Application.CertificateManage
{
    public class CertificateApp
    {
        private ItemsDetailApp itemsDetailApp = new ItemsDetailApp();
        List<ItemsDetailEntity> itemsDetailEntities = new List<ItemsDetailEntity>();
        private static string CERTTIFICATEFILEPATHS = Configs.GetValue("CertificateFilePaths");
        private ICertificateRepository service = new CertificateRepository();

        public CertificateApp()
        {
            itemsDetailEntities = itemsDetailApp.GetItemList("CertificateType");
        }

        public List<CertificateEntity> GetAllList()
        {
            return service.IQueryable().OrderBy(t => t.CreatorTime).ToList();
        }
        public List<CertificateEntity> GetList()
        {
            return service.IQueryable(m => m.DeleteMark != true).OrderBy(t => t.CreatorTime).ToList();
        }
        public List<CertificateEntity> GetListByType(string projectType)
        {
            return service.IQueryable(m => m.DeleteMark != true && m.ProjectType == projectType).OrderBy(t => t.CreatorTime).ToList();
        }
        public List<CertificateEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<CertificateEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.FullName.Contains(keyword));
                expression = expression.Or(t => t.IdCard.Contains(keyword));
                expression = expression.Or(t => t.ProjectName.Contains(keyword));
                expression = expression.Or(t => t.Number.Contains(keyword));
            }
            expression = expression.And(t => t.DeleteMark != true);
            return service.FindList(expression, pagination);
        }
        public List<CertificateEntity> GetList(Pagination pagination, string keyword, string projectType)
        {
            var expression = ExtLinq.True<CertificateEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.FullName.Contains(keyword));
                expression = expression.Or(t => t.IdCard.Contains(keyword));
                expression = expression.Or(t => t.ProjectName.Contains(keyword));
                expression = expression.Or(t => t.Number.Contains(keyword));
            }
            expression = expression.And(t => t.ProjectType == projectType);
            expression = expression.And(t => t.DeleteMark != true);
            List<CertificateEntity> models = service.FindList(expression, pagination);
            if (models != null)
            {
                foreach (var item in models)
                {
                    ItemsDetailEntity itemsDetailEntity = itemsDetailEntities.Where(m => m.ItemCode == item.ProjectType).FirstOrDefault();
                    if (itemsDetailEntity != null && !string.IsNullOrEmpty(itemsDetailEntity.Id))
                    {
                        item.ProjectName = itemsDetailEntity.ItemName;
                    }
                }
            }
            return models;
        }
        public List<CertificateEntity> GetEnableList()
        {
            return service.IQueryable(m => m.DeleteMark != true && m.EnabledMark == true).OrderBy(t => t.CreatorTime).ToList();
        }
        public CertificateEntity GetForm(string keyValue)
        {
            CertificateEntity certificateEntity = service.FindEntity(keyValue);
            ItemsDetailEntity itemsDetailEntity = itemsDetailEntities.Where(m => m.ItemCode == certificateEntity.ProjectType).FirstOrDefault();
            if (itemsDetailEntity != null && !string.IsNullOrEmpty(itemsDetailEntity.Id))
            {
                certificateEntity.ProjectName = itemsDetailEntity.ItemName;
            }
            return certificateEntity;
        }
        public CertificateEntity GetForm(string name, string idCard)
        {
            CertificateEntity certificateEntity = service.IQueryable(m => m.DeleteMark != true
            && m.FullName == name && m.IdCard == idCard).OrderBy(t => t.CreatorTime).FirstOrDefault();
            if (certificateEntity == null)
                certificateEntity = new CertificateEntity();
            else
            {
                ItemsDetailEntity itemsDetailEntity = itemsDetailEntities.Where(m => m.ItemCode == certificateEntity.ProjectType).FirstOrDefault();
                if (itemsDetailEntity != null && !string.IsNullOrEmpty(itemsDetailEntity.Id))
                {
                    certificateEntity.ProjectName = itemsDetailEntity.ItemName;
                }
            }
            return certificateEntity;
        }
        public List<CertificateEntity> GetForms(string name, string idCard)
        {
            List<CertificateEntity> certificateEntitys = service.IQueryable(m => m.DeleteMark != true
            && m.FullName == name && m.IdCard == idCard).OrderBy(t => t.CreatorTime).ToList();
            if (certificateEntitys != null)
            {
                foreach (var item in certificateEntitys)
                {
                    ItemsDetailEntity itemsDetailEntity = itemsDetailEntities.Where(m => m.ItemCode == item.ProjectType).FirstOrDefault();
                    if (itemsDetailEntity != null && !string.IsNullOrEmpty(itemsDetailEntity.Id))
                    {
                        item.ProjectName = itemsDetailEntity.ItemName;
                    }
                }
            }
            return certificateEntitys;
        }
        public CertificateEntity GetFormByNumber(string number)
        {
            CertificateEntity certificateEntity = service.IQueryable(m => m.DeleteMark != true
            && m.Number == number).OrderBy(t => t.CreatorTime).FirstOrDefault();
            if (certificateEntity == null)
                certificateEntity = new CertificateEntity();
            else
            {
                ItemsDetailEntity itemsDetailEntity = itemsDetailEntities.Where(m => m.ItemCode == certificateEntity.ProjectType).FirstOrDefault();
                if (itemsDetailEntity != null && !string.IsNullOrEmpty(itemsDetailEntity.Id))
                {
                    certificateEntity.ProjectName = itemsDetailEntity.ItemName;
                }
            }
            return certificateEntity;
        }
        public CertificateEntity GetFormByIdCard(string idCard)
        {
            CertificateEntity certificateEntity = service.IQueryable(m => m.DeleteMark != true
            && m.IdCard == idCard).OrderBy(t => t.CreatorTime).FirstOrDefault();
            if (certificateEntity == null)
                certificateEntity = new CertificateEntity();
            else
            {
                ItemsDetailEntity itemsDetailEntity = itemsDetailEntities.Where(m => m.ItemCode == certificateEntity.ProjectType).FirstOrDefault();
                if (itemsDetailEntity != null && !string.IsNullOrEmpty(itemsDetailEntity.Id))
                {
                    certificateEntity.ProjectName = itemsDetailEntity.ItemName;
                }
            }
            return certificateEntity;
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteById(t => t.Id == keyValue);
        }
        public void DeleteForms(string ids)
        {
            string[] sids = ids.Split(',');
            if (sids != null && sids.Length > 0)
            {
                for (int i = 0; i < sids.Length; i++)
                {
                    string id = sids[i];
                    service.DeleteById(t => t.Id == id);
                }
            }
        }
        public void SubmitForm(CertificateEntity certificateEntity, string keyValue, string projectType)
        {
            List<CertificateEntity> models = GetList();
            List<CertificateEntity> modelsT = models.Where(m => m.IdCard == certificateEntity.IdCard && m.Id != keyValue).ToList();
            if (modelsT != null && modelsT.Count > 0)
            {
                List<CertificateEntity> modelsT2 = modelsT.Where(m => m.Number == certificateEntity.Number && m.Id != keyValue).ToList();
                if (modelsT2 != null && modelsT2.Count > 0)
                {
                    throw new Exception("相同身份证号下证件编号已存在，请重新输入！");
                }
            }
            ItemsDetailEntity itemsDetailEntity = itemsDetailEntities.Where(m => m.ItemCode == projectType).FirstOrDefault();
            if (itemsDetailEntity != null && !string.IsNullOrEmpty(itemsDetailEntity.Id))
            {
                certificateEntity.ProjectName = itemsDetailEntity.ItemName;
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                certificateEntity.Modify(keyValue);
                service.Update(certificateEntity);
            }
            else
            {
                certificateEntity.Create();
                service.Insert(certificateEntity);
            }
        }
        public void AddForms(List<CertificateImportEntity> mdoels)
        {
            if (mdoels != null && mdoels.Count > 0)
            {
                foreach (CertificateImportEntity item in mdoels)
                {
                    if (item.IsQualified == true)
                    {
                        CertificateEntity certificateEntity = new CertificateEntity();
                        certificateEntity.SortCode = item.SortCode;
                        certificateEntity.FullName = item.FullName;
                        certificateEntity.Gender = item.Gender;
                        certificateEntity.IdCard = item.IdCard;
                        certificateEntity.ProjectType = item.ProjectType;
                        certificateEntity.ProjectName = item.ProjectName;
                        certificateEntity.Number = item.Number;
                        certificateEntity.CertificationTime = item.CertificationTime;
                        AddForm(certificateEntity);
                    }
                }
            }
        }
        public void AddForm(CertificateEntity certificateEntity)
        {
            List<CertificateEntity> models = GetList();
            List<CertificateEntity> modelsT = models.Where(m => m.IdCard == certificateEntity.IdCard).ToList();
            if (modelsT != null && modelsT.Count > 0)
            {
                List<CertificateEntity> modelsT2 = modelsT.Where(m => m.Number == certificateEntity.Number).ToList();
                if (modelsT2 != null && modelsT2.Count > 0)
                {
                    return;
                }
            }
            certificateEntity.Create();
            service.Insert(certificateEntity);
        }

        public MemoryStream ExportExcel(string projectType)
        {
            MemoryStream ms = new MemoryStream();
            List<CertificateEntity> models = GetListByType(projectType);
            if (models != null && models.Count > 0)
            {
                DataTable dataTable = GenDataTable(models);
                ms = ExcelHelper.DataTableToExcelForXLSX(dataTable);
            }
            return ms;
        }

        public List<CertificateImportEntity> UploadFiles(HttpFileCollectionBase Files, string projectType)
        {
            try
            {
                List<CertificateImportEntity> models = new List<CertificateImportEntity>();
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
                        if (fileExtension != ".xlsx")
                        {
                            throw new Exception("上传文件不合法，只能上传.xlsx文件");
                        }
                        string saveName = fileName; // 保存文件名称
                        string fileDirPaths = upPaths + "/" + controlIds + "/";
                        string filePaths = fileDirPaths + saveName;
                        string fullPaths = basePath + fileDirPaths;
                        Code.FileHelper.CreateDirectory(fullPaths);

                        Files[i].SaveAs(fullPaths + saveName);
                        models = ImportExcel(fullPaths + saveName, projectType);
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
            strPath = CERTTIFICATEFILEPATHS + dates + "/";
            return strPath;
        }

        private List<CertificateImportEntity> ImportExcel(string filePaths, string projectType)
        {
            string messageres = string.Empty;
            string message = string.Empty;
            List<CertificateImportEntity> models = GetExcelModels(filePaths, projectType);
            if (models != null && models.Count > 0)
            {
                foreach (CertificateImportEntity item in models)
                {
                    item.Id = Guid.NewGuid().ToString();
                    if (item.IsQualified == true)
                    {
                        List<CertificateEntity> modelsAll = GetList();
                        List<CertificateEntity> modelsT = modelsAll.Where(m => m.IdCard == item.IdCard).ToList();
                        if (modelsT != null && modelsT.Count > 0)
                        {
                            List<CertificateEntity> modelsT2 = modelsT.Where(m => m.Number == item.Number).ToList();
                            if (modelsT2 != null && modelsT2.Count > 0)
                            {
                                item.IsQualified = false;
                            }
                        }
                    }
                }
            }
            return models;
        }

        private List<CertificateImportEntity> GetExcelModels(string filePaths, string projectType)
        {
            try
            {
                List<CertificateImportEntity> models = new List<CertificateImportEntity>();
                DataTable tables = ExcelHelper.ExcelToTableForXLSX2(filePaths);
                if (tables != null && tables.Rows != null && tables.Rows.Count > 0)
                {
                    foreach (DataRow item in tables.Rows)
                    {
                        CertificateImportEntity certificateEntity = new CertificateImportEntity();
                        certificateEntity.IsQualified = true;
                        certificateEntity.ProjectType = projectType;
                        string projectName = string.Empty;
                        ItemsDetailEntity itemsDetailEntity = itemsDetailEntities.Where(m => m.ItemCode == projectType).FirstOrDefault();
                        if (itemsDetailEntity != null && !string.IsNullOrEmpty(itemsDetailEntity.Id))
                        {
                            projectName = itemsDetailEntity.ItemName;
                        }
                        if (item[0] == null || string.IsNullOrEmpty(item[0].ToString()))
                        {
                            certificateEntity.IsQualified = false;
                        }
                        if (item[0] != null)
                        {
                            string sortCodes = item[0].ToString();
                            int sortCode = 0;
                            if (int.TryParse(sortCodes, out sortCode))
                                certificateEntity.SortCode = sortCode;
                        }
                        if (item[1] == null || string.IsNullOrEmpty(item[1].ToString()))
                        {
                            certificateEntity.IsQualified = false;
                        }
                        if (item[1] != null)
                        {
                            string fullName = item[1].ToString();
                            certificateEntity.FullName = fullName;
                        }
                        if (item[2] == null || string.IsNullOrEmpty(item[2].ToString())
                            || (item[2].ToString() != "男" && item[2].ToString() != "女"))
                        {
                            certificateEntity.IsQualified = false;
                        }
                        if (item[2] != null)
                        {
                            string genders = item[2].ToString();
                            certificateEntity.Gender = -1;
                            if (genders == "男")
                            {
                                certificateEntity.Gender = 0;
                            }
                            if (genders == "女")
                            {
                                certificateEntity.Gender = 1;
                            }
                        }
                        if (item[3] == null || string.IsNullOrEmpty(item[3].ToString()))
                        {
                            certificateEntity.IsQualified = false;
                        }
                        if (item[3] != null)
                        {
                            string str = item[3].ToString();
                            certificateEntity.IdCard = str;
                        }
                        if (item[4] == null || string.IsNullOrEmpty(item[4].ToString()) || projectName != item[4].ToString())
                        {
                            certificateEntity.IsQualified = false;
                        }
                        if (item[4] != null)
                        {
                            string str = item[4].ToString();
                            certificateEntity.ProjectName = str;
                        }
                        if (item[5] == null || string.IsNullOrEmpty(item[5].ToString()))
                        {
                            certificateEntity.IsQualified = false;
                        }
                        if (item[5] != null)
                        {
                            string str = item[5].ToString();
                            certificateEntity.Number = str;
                        }
                        if (item[6] == null || string.IsNullOrEmpty(item[6].ToString()))
                        {
                            certificateEntity.IsQualified = false;
                        }
                        if (item[6] != null)
                        {
                            string str = item[6].ToString();
                            certificateEntity.CertificationTime = str;
                        }
                        models.Add(certificateEntity);
                    }
                }
                return models;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 实体类转换成DataTable
        /// </summary>
        /// <param name="modelList">实体类列表</param>
        /// <returns></returns>
        public DataTable GenDataTable(List<CertificateEntity> models)
        {
            if (models == null || models.Count == 0)
            {
                return null;
            }
            DataTable dt = CreateData();

            foreach (CertificateEntity model in models)
            {
                DataRow dataRow = dt.NewRow();
                dataRow[0] = model.SortCode;
                dataRow[1] = model.FullName;
                if (model.Gender == 0)
                {
                    dataRow[2] = "男";
                }
                else
                {
                    dataRow[2] = "女";
                }
                dataRow[3] = model.IdCard;
                dataRow[4] = model.ProjectName;
                dataRow[5] = model.Number;
                dataRow[6] = model.CertificationTime;
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// 根据实体类得到表结构
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        private DataTable CreateData()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("序号"));
            dataTable.Columns.Add(new DataColumn("姓名"));
            dataTable.Columns.Add(new DataColumn("性别"));
            dataTable.Columns.Add(new DataColumn("身份证号"));
            dataTable.Columns.Add(new DataColumn("证书类型"));
            dataTable.Columns.Add(new DataColumn("证书编号"));
            dataTable.Columns.Add(new DataColumn("发证时间"));
            return dataTable;
        }
    }
}
