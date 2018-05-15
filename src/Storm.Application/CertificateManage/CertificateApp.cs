using Storm.Code;
using Storm.Domain.Entity.CertificateManage;
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
        private static string CERTTIFICATEFILEPATHS = Configs.GetValue("CertificateFilePaths");
        private ICertificateRepository service = new CertificateRepository();

        public List<CertificateEntity> GetAllList()
        {
            return service.IQueryable().OrderBy(t => t.CreatorTime).ToList();
        }
        public List<CertificateEntity> GetList()
        {
            return service.IQueryable(m => m.DeleteMark != true).OrderBy(t => t.CreatorTime).ToList();
        }
        public List<CertificateEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<CertificateEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.FullName.Contains(keyword));
                expression = expression.Or(t => t.IdCard.Contains(keyword));
                expression = expression.Or(t => t.ProjectName.Contains(keyword));
                expression = expression.Or(t => t.ProjectType.Contains(keyword));
                expression = expression.Or(t => t.Number.Contains(keyword));
            }
            expression = expression.And(t => t.DeleteMark != true);
            return service.FindList(expression, pagination);
        }
        public List<CertificateEntity> GetEnableList()
        {
            return service.IQueryable(m => m.DeleteMark != true && m.EnabledMark == true).OrderBy(t => t.CreatorTime).ToList();
        }
        public CertificateEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public CertificateEntity GetForm(string name, string idCard)
        {
            CertificateEntity certificateEntity = service.IQueryable(m => m.DeleteMark != true
            && m.FullName == name && m.IdCard == idCard).OrderBy(t => t.CreatorTime).FirstOrDefault();
            if (certificateEntity == null)
                certificateEntity = new CertificateEntity();
            return certificateEntity;
        }
        public List<CertificateEntity> GetForms(string name, string idCard)
        {
            List<CertificateEntity> certificateEntitys = service.IQueryable(m => m.DeleteMark != true
            && m.FullName == name && m.IdCard == idCard).OrderBy(t => t.CreatorTime).ToList();
            return certificateEntitys;
        }
        public CertificateEntity GetFormByNumber(string number)
        {
            CertificateEntity certificateEntity = service.IQueryable(m => m.DeleteMark != true
            && m.Number == number).OrderBy(t => t.CreatorTime).FirstOrDefault();
            if (certificateEntity == null)
                certificateEntity = new CertificateEntity();
            return certificateEntity;
        }
        public CertificateEntity GetFormByIdCard(string idCard)
        {
            CertificateEntity certificateEntity = service.IQueryable(m => m.DeleteMark != true
            && m.IdCard == idCard).OrderBy(t => t.CreatorTime).FirstOrDefault();
            if (certificateEntity == null)
                certificateEntity = new CertificateEntity();
            return certificateEntity;
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteById(t => t.Id == keyValue);
        }
        public void SubmitForm(CertificateEntity certificateEntity, string keyValue)
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

        public MemoryStream ExportExcel()
        {
            MemoryStream ms = new MemoryStream();
            List<CertificateEntity> models = GetList();
            if (models != null && models.Count > 0)
            {
                DataTable dataTable = GenDataTable(models);
                ms = ExcelHelper.DataTableToExcelForXLSX(dataTable);
            }
            return ms;
        }

        public List<CertificateImportEntity> UploadFiles(HttpFileCollectionBase Files)
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
                        models = ImportExcel(fullPaths + saveName);
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

        private List<CertificateImportEntity> ImportExcel(string filePaths)
        {
            string messageres = string.Empty;
            string message = string.Empty;
            List<CertificateImportEntity> models = GetExcelModels(filePaths);
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

        private List<CertificateImportEntity> GetExcelModels(string filePaths)
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
                        if (item[0] == null || string.IsNullOrEmpty(item[0].ToString()))
                        {
                            certificateEntity.IsQualified = false;
                        }
                        else
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
                        else
                        {
                            string fullName = item[1].ToString();
                            certificateEntity.FullName = fullName;
                        }
                        if (item[2] == null || string.IsNullOrEmpty(item[2].ToString()))
                        {
                            certificateEntity.IsQualified = false;
                        }
                        else
                        {
                            string genders = item[2].ToString();
                            if (genders == "男")
                            {
                                certificateEntity.Gender = 0;
                            }
                            else
                            {
                                certificateEntity.Gender = 1;
                            }
                        }
                        if (item[3] == null || string.IsNullOrEmpty(item[3].ToString()))
                        {
                            certificateEntity.IsQualified = false;
                        }
                        else
                        {
                            string str = item[3].ToString();
                            certificateEntity.IdCard = str;
                        }
                        if (item[4] == null || string.IsNullOrEmpty(item[4].ToString()))
                        {
                            certificateEntity.IsQualified = false;
                        }
                        else
                        {
                            string str = item[4].ToString();
                            certificateEntity.ProjectName = str;
                        }
                        if (item[5] == null || string.IsNullOrEmpty(item[5].ToString()))
                        {
                            certificateEntity.IsQualified = false;
                        }
                        else
                        {
                            string str = item[5].ToString();
                            certificateEntity.ProjectType = str;
                        }
                        if (item[6] == null || string.IsNullOrEmpty(item[6].ToString()))
                        {
                            certificateEntity.IsQualified = false;
                        }
                        else
                        {
                            string str = item[6].ToString();
                            certificateEntity.Number = str;
                        }
                        if (item[7] == null || string.IsNullOrEmpty(item[7].ToString()))
                        {
                            certificateEntity.IsQualified = false;
                        }
                        else
                        {
                            string str = item[7].ToString();
                            DateTime dateTime = DateTime.Now;
                            if (DateTime.TryParse(str, out dateTime))
                                certificateEntity.CertificationTime = dateTime;
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
                dataRow[5] = model.ProjectType;
                dataRow[6] = model.Number;
                if (model.CertificationTime != null)
                {
                    dataRow[7] = ((DateTime)model.CertificationTime).ToString("yyyy-MM-dd");
                }
                else
                {
                    dataRow[7] = "";
                }
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
            dataTable.Columns.Add(new DataColumn("项目名称"));
            dataTable.Columns.Add(new DataColumn("证书类型"));
            dataTable.Columns.Add(new DataColumn("证书编号"));
            dataTable.Columns.Add(new DataColumn("发证时间"));
            return dataTable;
        }
    }
}
