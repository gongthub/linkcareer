﻿using Storm.Application.CertificateManage;
using Storm.Code;
using Storm.Domain.Entity.CertificateManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Storm.Web.Areas.CertificateManage.Controllers
{
    public class CertificateController : ControllerBase
    {
        private CertificateApp certificateApp = new CertificateApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = certificateApp.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = certificateApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(CertificateEntity certificateEntity, string keyValue)
        {
            certificateApp.SubmitForm(certificateEntity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            certificateApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormByNameAndIdCard(string name, string idCard)
        {
            var data = certificateApp.GetForm(name, idCard);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormByNumber(string number)
        {
            var data = certificateApp.GetFormByNumber(number);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAuthorize]
        public ActionResult Import()
        {
            try
            {
                string message = "导入成功！";
                if (HttpContext.Request.Files.Count > 0)
                {
                    var upFiles = HttpContext.Request.Files;
                    if (upFiles != null)
                    {
                        string messageres = certificateApp.UploadFiles(upFiles);
                        if (!string.IsNullOrEmpty(messageres))
                        {
                            message = message + "身份证号：" + messageres + "已存在！";
                        }
                    }
                }
                else
                {
                    return Success("false", "必须选择一个文件");
                }
                return Success("true", message);

            }
            catch (Exception ex)
            {
                return Success("false", ex.Message);
            }
        }


        [HttpGet]
        public void Export()
        {
            MemoryStream ms = new MemoryStream();
            ms = certificateApp.ExportExcel();
            string sheetName = HttpUtility.UrlEncode("证书", System.Text.Encoding.UTF8);
            HttpContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" + sheetName + ".xlsx");
            HttpContext.Response.BinaryWrite(ms.ToArray());
            HttpContext.Response.End();
        }
    }
}