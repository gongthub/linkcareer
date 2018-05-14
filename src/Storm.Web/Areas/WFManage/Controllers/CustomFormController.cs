using Storm.Application.WFManage;
using Storm.Code;
using Storm.Domain.Entity.WFManage;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Storm.Web.Areas.WFManage.Controllers
{
    public class CustomFormController : ControllerBase
    {
        private FormApp formApp = new FormApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = formApp.GetList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetEnableGridJson(string keyword)
        {
            var data = formApp.GetEnableList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = formApp.GetForm(keyValue);
            data.Codes = Server.HtmlDecode(data.Codes);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(FormEntity formEntity, string keyValue)
        {
            formApp.SubmitForm(formEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            formApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult EnbaledForm(string keyValue)
        {
            formApp.EnbaledForm(keyValue);
            return Success("启用成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledForm(string keyValue)
        {
            formApp.DisabledForm(keyValue);
            return Success("禁用成功。");
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Design()
        {
            return View();
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SaveDesign(string keyValue, string codes)
        {
            List<FormControlEntity> formControlModels = new List<FormControlEntity>();
            string formControls = Request["formControls"];
            if (!string.IsNullOrEmpty(formControls))
            {
                formControlModels = formControls.ToObject<List<FormControlEntity>>();
            }
            formApp.SaveDesign(keyValue, codes, formControlModels);
            return Success("保存成功。");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetCommonDefaultProgramsJson(string flowId)
        {
            List<EnumModel> models = formApp.GetCommonDefaultPrograms(flowId);
            return Content(models.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetCommonCustomDefaultValuesJson(string flowId, string controlId)
        {
            string defaultValues = formApp.GetCommonCustomDefaultValuesJson(flowId, controlId);
            return Content(defaultValues);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetCommonCustomDefaultTypeJson(string flowId, string controlId)
        {
            string defaultTypes = formApp.GetCommonCustomDefaultTypeJson(flowId, controlId);
            return Content(defaultTypes);
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetCommonCustomDefaultTypeByWorkIdJson(string workId, string controlId)
        {
            string defaultTypes = formApp.GetCommonCustomDefaultTypeByWorkIdJson(workId, controlId);
            return Content(defaultTypes);
        }
    }
}
