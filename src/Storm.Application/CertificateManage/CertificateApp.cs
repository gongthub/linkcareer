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
        public CertificateEntity GetFormByNumber(string number)
        {
            CertificateEntity certificateEntity = service.IQueryable(m => m.DeleteMark != true
            && m.Number == number).OrderBy(t => t.CreatorTime).FirstOrDefault();
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

        public List<CertificateEntity> UploadFiles(HttpFileCollectionBase Files)
        {
            try
            {
                List<CertificateEntity> models = new List<CertificateEntity>();
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
                        string saveName = fileName + fileExtension; // 保存文件名称
                        string fileDirPaths = upPaths + "/" + controlIds + "/";
                        string filePaths = fileDirPaths + saveName;
                        string fullPaths = basePath + fileDirPaths;
                        Code.FileHelper.CreateDirectory(fullPaths);

                        Files[i].SaveAs(fullPaths + saveName);
                        ImportExcel(fullPaths + saveName);
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

        private void ImportExcel(string filePaths)
        {
            List<CertificateEntity> models = GetExcelModels(filePaths);
        }

        private List<CertificateEntity> GetExcelModels(string filePaths)
        {
            List<CertificateEntity> models = new List<CertificateEntity>();
            DataTable tables = ExcelHelper.ExcelToTableForXLSX(filePaths);
            if (tables != null && tables.Rows != null && tables.Rows.Count > 0)
            {
                foreach (DataRow item in tables.Rows)
                {
                    CertificateEntity certificateEntity = new CertificateEntity();
                    if (item[0] != null)
                    {
                        string sortCodes = item[0].ToString();
                        int sortCode = 0;
                        if (int.TryParse(sortCodes, out sortCode))
                            certificateEntity.SortCode = sortCode;
                    }
                }
            }
            return models;
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
                if (model.CertificationTime != null)
                {
                    dataRow[6] = ((DateTime)model.CertificationTime).ToString("yyyy-MM-dd");
                }
                else
                {
                    dataRow[6] = "";
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
            dataTable.Columns.Add(new DataColumn("证书编号"));
            dataTable.Columns.Add(new DataColumn("发证时间"));
            return dataTable;
        }
    }
}
