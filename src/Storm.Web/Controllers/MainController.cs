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
            List<CertificateEntity> certificateEntitys = new List<CertificateEntity>();
            List<CertificateShowEntity> certificateShowEntitys = new List<CertificateShowEntity>();
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(IdNumber))
            {
                certificateEntitys = certificateApp.GetForms(name, IdNumber);
            }
            if (!string.IsNullOrEmpty(CertificateNumber))
            {
                certificateEntitys = new List<CertificateEntity>();
                CertificateEntity certificateEntitT = certificateApp.GetFormByNumber(CertificateNumber);
                if (certificateEntitT != null && !string.IsNullOrEmpty(certificateEntitT.Id))
                    certificateEntitys.Add(certificateEntitT);
            }
            if (certificateEntitys != null && certificateEntitys.Count > 0)
            {
                foreach (CertificateEntity item in certificateEntitys)
                {
                    CertificateShowEntity certificateShowEntity = new CertificateShowEntity();
                    certificateShowEntity.SortCode = item.SortCode;
                    certificateShowEntity.FullName = item.FullName;

                    certificateShowEntity.IdCard = item.IdCard;
                    certificateShowEntity.ProjectType = item.ProjectType;
                    certificateShowEntity.ProjectName = item.ProjectName;
                    certificateShowEntity.Number = item.Number;
                    if (item.Gender == 0)
                    {
                        certificateShowEntity.Gender = "男";
                    }
                    if (item.Gender == 1)
                    {
                        certificateShowEntity.Gender = "女";
                    }
                    if (item.CertificationTime != null)
                    {
                        certificateShowEntity.CertificationTime = item.CertificationTime;
                    }
                    certificateShowEntitys.Add(certificateShowEntity);
                }
            }
            if (certificateShowEntitys != null && certificateShowEntitys.Count > 0)
                return View(certificateShowEntitys);
            else
                return View("NoFind");
        }
        // GET: Main
        public ActionResult NoFind()
        {
            return View();
        }
    }
}