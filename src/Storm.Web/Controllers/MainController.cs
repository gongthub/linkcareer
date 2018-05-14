using Storm.Application.CertificateManage;
using Storm.Domain.Entity.CertificateManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Storm.Web.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string name, string IdNumber, string CertificateNumber)
        {
            CertificateApp certificateApp = new CertificateApp();
            CertificateEntity certificateEntity = new CertificateEntity();
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(IdNumber))
            {
                certificateEntity = certificateApp.GetForm(name, IdNumber);
            }
            if (!string.IsNullOrEmpty(CertificateNumber))
            {
                certificateEntity = certificateApp.GetFormByNumber(CertificateNumber);
            }
            ViewBag.GenderStr = "";
            if (certificateEntity != null)
            {
                if (certificateEntity.Gender == 0)
                {
                    ViewBag.GenderStr = "男";
                }
                if (certificateEntity.Gender == 1)
                {
                    ViewBag.GenderStr = "女";
                }
            }
            ViewBag.CertificationTimeStr = "";
            if (certificateEntity != null)
            {
                if (certificateEntity.CertificationTime != null)
                {
                    ViewBag.CertificationTimeStr = ((DateTime)certificateEntity.CertificationTime).ToString("yyyy-MM-dd");
                }
            }
            return View(certificateEntity);
        }
    }
}