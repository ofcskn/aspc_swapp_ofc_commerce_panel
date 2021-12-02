using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace www.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorCode = "404";
                    ViewBag.ErrorMessage = "Aradığınız sayfayı bulamadık. Lütfen farklı bir sayfa aratın.";
                    break;

                case 500:
                    ViewBag.ErrorCode = "500";
                    ViewBag.ErrorMessage = "Ooops! Bir şeyler yanlış gitti. Lütfen site sahibi ile iletişime geçin.";
                    break;

                case 403:
                    ViewBag.ErrorCode = "403";
                    ViewBag.ErrorMessage = "Tüh! Erişim Sorunu. Lütfen site sahibi ile iletişime geçin.";
                    break;
            }
            return View();
        }
    }
}
