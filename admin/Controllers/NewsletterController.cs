using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using admin.ViewModel;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using Service.Utilities;

namespace admin.Controllers
{
    [Authorize(Policy = "Admin")]
    public class NewsletterController : Controller
    {
        private readonly IUnitOfWork _uow;
        public NewsletterController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpPost]
        public JsonResult PublishStatus(bool enabled, int itemId)
        {
            var item = _uow.Newsletter.GetById(itemId);
            if (item != null)
            {
                item.Enabled = enabled;
                _uow.Newsletter.Update(item);
                if (enabled == true)
                {
                    return Json("start");
                }
                return Json("pause");
            }
            return Json("no");
        }
        [HttpGet]
        public IActionResult List()
        {
            ////Add Notification
            //Notification notification = new Notification
            //{
            //    Enabled = false,
            //    NotTypeId = 1,
            //    SendDate = DateTime.Now,
            //    Title = _uow.Admin.GetUserNameSurname(_uow.Admin.GetRoleByAdmin, _uow.Admin.GetIdByAdmin, "name") + "kişisi size bir email gönderdi.",
            //    UserId = entity.ReceiverId,
            //    UserRole = entity.ReceiverRole
            //};
            //_uow.Notification.Add(notification);
            return View(_uow.Newsletter.GetAll());
        }
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Delete(int id)
        {
            var item = _uow.Newsletter.GetById(Convert.ToInt32(id));
            if (item != null)
            {
                _uow.Newsletter.Delete(item);
                return RedirectToAction("List");
            }
            return View(item);
        }
    }
}
