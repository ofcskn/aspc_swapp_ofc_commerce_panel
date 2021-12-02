using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.Context;
using Entity.Models;
using Entity.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service.Abstract;
using Service.Utilities;
using Microsoft.AspNetCore.Authorization;
using admin.Models;

namespace admin.Controllers
{
    [Authorize(Policy = "Staff")]
    public class InvoiceController : Controller
    {
        private readonly IUnitOfWork _uow;
        public InvoiceController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpGet]
        public IActionResult Email(int? id)
        {
            EmailViewModel vm = new EmailViewModel();
            vm.Invoice = _uow.Invoice.GetInvoice(Convert.ToInt32(id));
            return View(vm);
        }
        public IActionResult List()
        {
            return View(_uow.Invoice.GetAllWithDiagram());
        }
        public IActionResult Detail(int id)
        {
            return View(_uow.Invoice.GetInvoiceById(id));
        }
        [HttpGet]
        public IActionResult Manage(int? id)
        {
            if (id != null)
            {
                return View(_uow.Invoice.GetById(Convert.ToInt32(id)));
            }
            return View(new Invoice());
        }
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Manage(Invoice entity)
        {
            if (entity.Id == 0)
            {
                if (ModelState.IsValid)
                {
                    var nottype = _uow.NotificationType.GetNTByType("send-invoice");
                    try
                    {
                        string notificationTitle = _uow.Admin.GetUserNameSurname("staff", Convert.ToInt32(entity.StaffId), "name") + " " + nottype.Message;
                        //Add Notification
                        Notification notification = new Notification
                        {
                            Enabled = false,
                            NotTypeId = nottype.Id,
                            SendDate = DateTime.Now,
                            Title = notificationTitle,
                            UserId = Convert.ToInt32(entity.CurrentId),
                            UserRole = "current"
                        };
                        _uow.Notification.Add(notification);
                    }
                    catch (Exception)
                    {
                        throw;
                    }



                    entity.SendDate = DateTime.Now;
                    _uow.Invoice.Add(entity);
                    return RedirectToAction("List");
                }
                return View(entity);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _uow.Invoice.Update(entity);
                    return RedirectToAction("List");
                }
                return View(entity);
            }
        }
        [Authorize(Policy = "Admin")]
        public IActionResult Delete(int id)
        {
            var item = _uow.Invoice.GetById(id);
            _uow.Invoice.Delete(item);
            return Json("ok");
        }
    }
}
