using Storm.Application.SystemManage;
using Storm.Code;
using Storm.Domain.Entity.SystemManage;
using Storm.Domain.Entity.WFManage;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Storm.Web.Areas.WFManage.Controllers
{
    public class CustomControlController : ControllerBase
    {
        private OrganizeApp organizeApp = new OrganizeApp();
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult TextControl()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult TextAreaControl()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult RedioControl()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult CheckBoxControl()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult HiddenControl()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult LableControl()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult SelectControl()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult ComboxControl()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult OrgControl()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult DateControl()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult DateTimeControl()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult FilesControl()
        {
            return View();
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult DefaultProgram()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetDefaultProgramsJson(string keyword)
        {
            List<EnumModel> models = EnumHelp.enumHelp.EnumToList(typeof(FormDefaultProgram));
            return Content(models.ToJson());
        }
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult OrgForm()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetOrgJson(string keyword)
        {
            var data = organizeApp.GetEnableList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.FullName.Contains(keyword));
            }
            var treeList = new List<TreeGridModel>();
            foreach (OrganizeEntity item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = data.Count(t => t.ParentId == item.Id) == 0 ? false : true;
                treeModel.id = item.Id;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.ParentId;
                treeModel.expanded = hasChildren;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeGridJson());
        }
        
    }
}
