using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using admin.Models;
using Entity.Models;
using Entity.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service.Abstract;
using Service.Utilities;

namespace admin.Controllers
{
    [Authorize(Policy = "User")]
    public class EmailDesignController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IViewRenderService _viewRenderService;
        private readonly IConfiguration _configuration;
        public EmailDesignController(IUnitOfWork uow, IViewRenderService viewRenderService, IConfiguration configuration)
        {
            _uow = uow;
            _viewRenderService = viewRenderService;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult Index(int? id, string filter)
        {
            if (filter == "invoice")
            {
                EmailViewModel vm = new EmailViewModel();
                vm.Invoice = _uow.Invoice.GetInvoice(Convert.ToInt32(id));
                vm.Type = "invoice";
                return View(vm);
            }
            else if (filter == "confirm")
            {
                EmailViewModel vm = new EmailViewModel();
                vm.Type = "invoice";
                return View(vm);
            }
            else if (filter == "newsletter")
            {
                EmailViewModel vm = new EmailViewModel();
                vm.Type = "newsletter";
                return View(vm);
            }
            else if (filter == "normal")
            {
                EmailViewModel vm = new EmailViewModel();
                vm.Type = "normal";
                return View(vm);
            }
            else
            {
                EmailViewModel vm = new EmailViewModel();
                return View(vm);
            }
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post(int? id, string filter, string BodyText, string Subject, IEnumerable<IFormFile> Files)
        {
            if (filter == "invoice")
            {
                Invoice entity = _uow.Invoice.GetInvoiceById(Convert.ToInt32(id));
                if (entity.DueDate == null)
                {
                    //Send Mail
                    var html = await _viewRenderService.RenderToStringAsync("EmailDesign/_InvoiceEmail", entity);
                    _uow.Common.SendMailToDefault("Swapp Fatura Bilgileriniz", html, entity.SendDate.ToString("d") + "Faturanız ektedir. İyi günler. Swapp'le kalın.", _configuration);
                    entity.DueDate = DateTime.Now;
                    _uow.Invoice.Update(entity);
                    return Json("ok");
                }
                return Json("no");
            }
            if (filter == "normal")
            {
                List<string> toMails = new List<string>();
                foreach (var item in _uow.Newsletter.GetAll().Where(p => p.Enabled == true))
                {
                    toMails.Add(item.Mail);
                }
                _uow.Common.SendMailMultipleWithMail(Subject, null, BodyText, _configuration, toMails, Files);
                return Redirect("/admin/newsletter/list");
            }
            return Json("no");
        }
    }
}
