using Storm.Application.WFManage;
using Storm.Code;
using Storm.Domain.Entity.WFManage;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Storm.Web.Areas.WFManage.Controllers
{
    public class FlowController : ControllerBase
    {
        private FlowApp flowApp = new FlowApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = flowApp.GetList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = flowApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormTypeSelectJson()
        {
            List<EnumModel> models = EnumHelp.enumHelp.EnumToList(typeof(FormType));
            return Content(models.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetStepTypeSelectJson()
        {
            List<EnumModel> models = EnumHelp.enumHelp.EnumToList(typeof(StepType));
            return Content(models.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetRejectTypeSelectJson()
        {
            List<EnumModel> models = EnumHelp.enumHelp.EnumToList(typeof(RejectType));
            return Content(models.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetReviewerTypeSelectJson()
        {
            List<EnumModel> models = EnumHelp.enumHelp.EnumToList(typeof(ReviewerType));
            return Content(models.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetMessageTypeSelectJson()
        {
            List<EnumModel> models = EnumHelp.enumHelp.EnumToList(typeof(MessageType));
            return Content(models.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetStrategiesTypeSelectJson()
        {
            List<EnumModel> models = EnumHelp.enumHelp.EnumToList(typeof(StrategiesType));
            return Content(models.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetApprovalStatusSelectJson()
        {
            List<EnumModel> models = EnumHelp.enumHelp.EnumToList(typeof(ApprovalStatus));
            return Content(models.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetDesignJson(string keyValue)
        {
            var data = flowApp.GetDesign(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(FlowEntity flowEntity, string keyValue)
        {
            flowApp.SubmitForm(flowEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            flowApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult EnbaledForm(string keyValue)
        {
            flowApp.EnbaledForm(keyValue);
            return Success("启用成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledForm(string keyValue)
        {
            flowApp.DisabledForm(keyValue);
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
            flowApp.SaveDesign(keyValue, codes);
            return Success("保存成功。");
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult FormStrategies()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult SqlStrategies()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetStrategieFormJson(string flowId, string markName)
        {
            var data = flowApp.GetLine(flowId, markName);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SaveFormStrategies(string flowId, string markName, int flotType, string plots)
        {
            flowApp.SaveStrategies(flowId, markName, flotType, plots);
            return Success("保存成功。");
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult ReviewerOrg()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult ReviewerPeople()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetReviewerFormJson(string flowId, string markName)
        {
            var data = flowApp.GetNode(flowId, markName);
            return Content(data.ToJson());
        }
    }
}
