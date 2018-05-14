using Storm.Application.WFManage;
using Storm.Code;
using Storm.Domain.Entity.WFManage;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace Storm.Web.Areas.WFManage.Controllers
{
    public class FlowDictionaryDetailController : ControllerBase
    {
        private WFItemApp itemsApp = new WFItemApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string itemId, string keyword)
        {
            var data = itemsApp.GetDetailList(itemId, keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSelectJsonByItemId(string itemId)
        {
            var data = itemsApp.GetItemDetailByItemIdList(itemId);
            List<object> list = new List<object>();
            foreach (WFItemDetailEntity item in data)
            {
                list.Add(new { id = item.Id, text = item.ItemName });
            }
            return Content(list.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = itemsApp.GetDetailForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(WFItemDetailEntity itemsDetailEntity, string keyValue)
        {
            itemsApp.SubmitDetailForm(itemsDetailEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            itemsApp.DeleteDetailForm(keyValue);
            return Success("删除成功。");
        }
    }
}
