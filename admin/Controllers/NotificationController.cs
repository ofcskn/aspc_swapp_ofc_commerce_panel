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
using Microsoft.EntityFrameworkCore;
using Service.Abstract;
using Service.Utilities;

namespace admin.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly IUnitOfWork _uow;
        public NotificationController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpPost]
        public JsonResult IsRead(int id)
        {
            var entity = _uow.Notification.GetById(Convert.ToInt32(id));
            if (entity.ReadEnabled == false)
            {
                entity.ReadEnabled = true;
                entity.ReadDate = DateTime.Now;
                _uow.Notification.Update(entity);
                return Json("is-read");
            }
            else
            {
                entity.ReadEnabled = false;
                _uow.Notification.Update(entity);

                return Json("not-read");
            }
        }
        [HttpGet]
        public IActionResult List()
        {
            return View(_uow.Notification.GetAllByUser(_uow.Admin.GetIdByAdmin, _uow.Admin.GetRoleByAdmin).Include(p => p.NotType).OrderByDescending(p => p.SendDate));
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _uow.Notification.GetById(Convert.ToInt32(id));
            if (item != null)
            {
                _uow.Notification.Delete(item);
                return RedirectToAction("List");
            }
            return View(item);
        }

    }
}
