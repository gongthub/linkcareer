using Storm.Application.SystemSecurity;
using Storm.Code;
using Storm.Domain.Entity.SystemSecurity;
using System.Web.Mvc;

namespace Storm.Web.Areas.SystemSecurity.Controllers
{
    public class DbBackupController : ControllerBase
    {
        private DbBackupApp dbBackupApp = new DbBackupApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string queryJson)
        {
            var data = dbBackupApp.GetList(queryJson);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(DbBackupEntity dbBackupEntity)
        {
            dbBackupEntity.FilePath = Server.MapPath("~/Resource/DbBackup/" + dbBackupEntity.FileName + ".bak");
            dbBackupEntity.FileName = dbBackupEntity.FileName + ".bak";
            dbBackupApp.SubmitForm(dbBackupEntity);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            dbBackupApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
        [HttpPost]
        [HandlerAuthorize]
        public void DownloadBackup(string keyValue)
        {
            var data = dbBackupApp.GetForm(keyValue);
            string filename = Server.UrlDecode(data.FileName);
            string filepath = Server.MapPath(data.FilePath);
            if (FileDownHelper.FileExists(filepath))
            {
                FileDownHelper.DownLoadold(filepath, filename);
            }
        }
    }
}
