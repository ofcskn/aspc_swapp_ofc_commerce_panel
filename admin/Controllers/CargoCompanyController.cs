using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using admin.Models;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using Service.Utilities;

namespace admin.Controllers
{
    [Authorize]
    public class CargoCompanyController : Controller
    {
        private readonly IUnitOfWork _uow;
        public CargoCompanyController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpGet]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> List(int pageNumber = 1)
        {
            var list = _uow.CargoCompany.GetAllPC();//Your list
            return View(await PaginatedList<CargoCompany>.CreateAsync(list, pageNumber, 10));
        }
        [HttpGet]
        [Authorize(Policy = "User")]
        public IActionResult Manage(int? id)
        {
            if (id != 0)
            {
                return View(_uow.CargoCompany.GetById(Convert.ToInt32(id)));
            }
            else
            {
                return View(new CargoCompany());
            }
        }
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Manage(CargoCompany entity, IFormFile Image)
        {
            string filePath = "/img/cargo-company/";
            int quality = 80;
            int w = 500;
            int h = 500;
            if (entity.Id == 0)
            {
                var nottype = _uow.NotificationType.GetNTByType("new-cargo-company");
                string notificationTitle = entity.Title + " " + nottype.Message;
                try
                {
                    _uow.Notification.AddListForStaff(nottype.Id, notificationTitle, _uow.Staff.GetAllByEnabled());
                }
                catch (Exception)
                {
                }

                try
                {
                    _uow.Notification.AddListForAdmin(nottype.Id, notificationTitle, _uow.Admin.GetAllByEnabled(true));
                }
                catch (Exception)
                {
                }

                if (Image != null)
                {
                    entity.Image = _uow.Media.ChangeFileName(entity.Title, Image);
                    _uow.Media.InsertWithIFormFile(filePath, entity.Title, "admin", Image, "cargo-company", quality, w, h);
                }
                _uow.CargoCompany.Add(entity);
            }
            else
            {
                if (Image != null)
                {
                    if (_uow.Media.ChangeFileName(Image.FileName, Image) != entity.Image)
                    {
                        _uow.Media.DeleteImage(entity.Image, "admin", filePath);
                        entity.Image = _uow.Media.ChangeFileName(entity.Title, Image);
                        _uow.Media.InsertWithIFormFile(filePath, entity.Title, "admin", Image, "cargo-company", quality, w, h);
                    }
                }
                _uow.CargoCompany.Update(entity);
            }
            return RedirectToAction("List");
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _uow.CargoCompany.GetById(Convert.ToInt32(id));
            if (item != null)
            {
                var nottype = _uow.NotificationType.GetNTByType("deleted-cargo-company");
                string notificationTitle = item.Title + " " + nottype.Message;
                try
                {
                    _uow.Notification.AddListForStaff(nottype.Id, notificationTitle, _uow.Staff.GetAllByEnabled());
                }
                catch (Exception)
                {
                }

                try
                {
                    _uow.Notification.AddListForAdmin(nottype.Id, notificationTitle, _uow.Admin.GetAllByEnabled(true));
                }
                catch (Exception)
                {
                }

                #region deleteMedia
                _uow.Media.DeleteImage(item.Image, "admin", "\\img\\cargo-company\\");
                #endregion
                _uow.CargoCompany.Delete(item);
                return RedirectToAction("List");
            }
            return View(item);
        }
    }
}
