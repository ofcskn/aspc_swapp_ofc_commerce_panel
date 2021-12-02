using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace admin.Controllers
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
                    ViewBag.ErrorMessage = "Aradığınız sayfayı bulamadık.";
                    ViewBag.ErrorSearchMessage = "<p> Menüyü ya da arama formunu kullanarak istediğiniz yere gitmeyi deneyin!</p>";
                    break;
                
                case 500:
                    ViewBag.ErrorCode = "500";
                    ViewBag.ErrorMessage = "Ooops! Bir şeyler yanlış gitti.";
                    ViewBag.ErrorSearchMessage = "<p>Ooops! Bir şeyler yanlış gitti. Lütfen sorunu hızlıca çözmemiz için bize bildirin!</p>";
                    break;
                
                case 403:
                    ViewBag.ErrorCode = "403";
                    ViewBag.ErrorMessage = "Tüh! Erişim Sorunu.";
                    ViewBag.ErrorSearchMessage = "<p>Bu sayfaya erişiminiz kısıtlandırıldı. Lütfen farklı bir sayfaya ziyaret etmeyi deneyin ya da bize ulaşın.</p>";
                    break;
            }
            return View();
        }
    }
}
