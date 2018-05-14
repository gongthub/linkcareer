using Storm.Application.WFManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Storm.Code;
using Storm.Domain.Entity.WFManage;

namespace Storm.Web.Areas.WFManage.Controllers
{
    public class WorkController : ControllerBase
    {
        private WorkApp workApp = new WorkApp();
        private WorkFlowApp workFlowApp = new WorkFlowApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = workApp.GetList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = workApp.GetForm(keyValue);
            if (data != null && !string.IsNullOrEmpty(data.Contents))
            {
                data.Contents = Server.HtmlDecode(data.Contents);
            }
            if (data != null && !string.IsNullOrEmpty(data.Codes))
            {
                data.Codes = Server.HtmlDecode(data.Codes);
            }
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormDesignJosn(string keyValue)
        {
            var data = workApp.GetFormDesign(keyValue);
            var result = new { codes = Server.HtmlDecode(data) };
            return Content(result.ToJson());
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Apply()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult MyFlow()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult ApplyForm()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult ApplyDetails()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetMyFlowGridJson(string keyword)
        {
            var data = workApp.GetMyWorkList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetWorkFormResJson(string workId)
        {
            var controls = workApp.GetWorkControls(workId);
            var files = workApp.GetWorkFiles(workId);
            var data = new { controls = controls, files = files };
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(string flowId, int status, string contents)
        {
            string custrols = Request["custrols"];
            var files = Request.Files;
            List<WorkFileEntity> workFiles = workApp.UploadFiles(files);
            List<WorkControlEntity> workControls = new List<WorkControlEntity>();
            if (!string.IsNullOrEmpty(custrols))
            {
                workControls = custrols.ToObject<List<WorkControlEntity>>();
            }
            workApp.AddForm(flowId, status, contents, workControls, workFiles);
            if (status == 2)
            {
                return Success("提交成功。");
            }
            else
            {
                return Success("保存成功。");
            }
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateForm(string workId, int status, string contents)
        {
            string custrols = Request["custrols"];
            string removefileIds = Request["removefileIds"];
            var files = Request.Files;
            List<WorkFileEntity> workFiles = workApp.UploadFiles(files);
            List<WorkControlEntity> workControls = new List<WorkControlEntity>();
            if (!string.IsNullOrEmpty(custrols))
            {
                workControls = custrols.ToObject<List<WorkControlEntity>>();
            }
            List<string> removeFileIds = new List<string>();
            if (!string.IsNullOrEmpty(removefileIds))
            {
                removeFileIds = removefileIds.ToObject<List<string>>();
            }
            workApp.UpdateForm(workId, status, contents, workControls, workFiles, removeFileIds);
            if (status == 2)
            {
                return Success("提交成功。");
            }
            else
            {
                return Success("保存成功。");
            }
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitApply(string workId)
        {
            workFlowApp.Start(workId);
            return Success("提交成功。");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetMyPendingGridJson(string keyword)
        {
            var data = workApp.GetMyPendingList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult MyPending()
        {
            return View();
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult ApplyApproval()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetApproPocessridJson(string workId)
        {
            var data = workFlowApp.GetApproProcessList(workId);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Approval(string workId, int status, string desc)
        {
            workFlowApp.Approval(workId, status, desc);
            return Success("审核成功。");
        }
    }
}
