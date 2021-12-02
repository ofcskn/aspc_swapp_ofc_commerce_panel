using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using www.Models;
using Service.Utilities;
using Service.Abstract;
using Microsoft.AspNetCore.Http;

namespace www.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _uow;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(Contact entity)
        {
            if (ModelState.IsValid)
            {
                entity.Date = DateTime.Now;
                entity.Ip = _uow.Admin.GetIp();
                _uow.Contact.Add(entity);
                ViewBag.SuccessMessage = "Mesajınız başarıyla gönderilmiştir. Teşekkürler.";
                return View();
            }
            return View(entity);
        }
        [HttpPost]
        public JsonResult BeSubscribe(Newsletter entity)
        {
            if (ModelState.IsValid)
            {
                if (!_uow.Newsletter.GetAll().Any(p=>p.Mail == entity.Mail))
                {
                    entity.Date = DateTime.Now;
                    entity.Enabled = false;
                    entity.Ip = _uow.Admin.GetIp();
                    _uow.Newsletter.Add(entity);
                    return Json("Başarıyla abone oldunuz. Teşekkürler!");
                }
                return Json("Bu mail adresi ile kayıtlı bir abonemiz vardır!");
            }
            return Json("Aboneliğiniz bir aksaklıktan dolayı yapılamadı. Lütfen daha sorna tekrar deneyiniz.");
        }
        public IActionResult BlogDetail()
        {
            return View();
        }
        public IActionResult BlogList()
        {
            return View();
        }
        public IActionResult ProjectDetail()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ClickData(string permalink, string leavinginpage)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(1);
            DateTime dateTime = DateTime.Now;
            string userIp = _uow.PageAnalysis.GetUserIp;

            if (leavinginpage == "true")
            {
                PageAnalysis pageAnalysis = _uow.Cookie.GetPageAnalysisId();
                if (pageAnalysis.EndDate == null)
                {
                    pageAnalysis.EndDate = dateTime;
                    _uow.PageAnalysis.Update(pageAnalysis);
                }
                return Json(pageAnalysis.EndDate);
            }
            else
            {
                PageAnalysis pa = new PageAnalysis
                {
                    Ip = userIp,
                    EntryDate = DateTime.Now,
                    Page = permalink,
                    Lang = "tr"
                };
                string encrypted = Cipher.Encrypt("SayfaAnaliz", "PageAnalysis");
                var analysisIdCookie = _uow.Cookie.GetCookie(encrypted);
                if (analysisIdCookie == null)
                {
                    _uow.Cookie.AddPAWithCookie(pa, encrypted, option);
                }
                else
                {
                    string analysisId = Cipher.Decrypt(analysisIdCookie, "PageAnalysis");
                    PageAnalysis paold = _uow.PageAnalysis.GetById(Convert.ToInt32(analysisId));
                    if (paold != null)
                    {
                        if ((DateTime.Now - paold.EntryDate).TotalMinutes > 2)
                        {
                            _uow.Cookie.AddPAWithCookie(pa, encrypted, option);
                        }
                    }
                    else
                    {
                        _uow.Cookie.AddPAWithCookie(pa, encrypted, option);
                    }
                }
                return Json("");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
