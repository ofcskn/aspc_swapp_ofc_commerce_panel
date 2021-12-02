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
    [Authorize(Policy = "Admin")]
    public class ProjectController : Controller
    {
        private readonly IUnitOfWork _uow;
        public ProjectController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpPost]
        public JsonResult PublishStatus(bool enabled, int itemId)
        {
            var item = _uow.Project.GetById(itemId);
            if (item != null)
            {
                item.Enabled = enabled;
                _uow.Project.Update(item);
                if (enabled == true)
                {
                    return Json("start");
                }
                return Json("pause");
            }
            return Json("no");
        }
        [HttpGet]
        public async Task<IActionResult> List(int pageNumber = 1)
        {
            var list = _uow.Project.GetAll();//Your list
            return View(await PaginatedList<Project>.CreateAsync(list, pageNumber, 10));
        }
        [HttpGet]
        public IActionResult Manage(int? id)
        {
            if (id != 0)
            {
                return View(_uow.Project.GetById(Convert.ToInt32(id)));
            }
            else
            {
                return View(new Project());
            }
        }
        [HttpPost]
        public IActionResult Manage(Project entity, IFormFile Image)
        {
            string filePath = "/img/project/";
            int quality = 60;
            int w = 1920;
            int h = 1080;
            if (entity.Id == 0)
            {
                entity.Image = _uow.Media.ChangeFileName(entity.Title, Image);
                _uow.Media.InsertWithIFormFile(filePath, entity.Title, "www", Image, "project", quality, w, h);
                entity.Priority = _uow.Project.GetAll().Max(p => p.Priority) + 1;
                _uow.Project.Add(entity);
            }
            else
            {
                if (Image != null)
                {
                    if (_uow.Media.ChangeFileName(Image.FileName, Image) != entity.Image)
                    {
                        _uow.Media.DeleteImage(entity.Image, "www", filePath);
                        entity.Image = _uow.Media.ChangeFileName(entity.Title, Image);
                        _uow.Media.InsertWithIFormFile(filePath, entity.Title, "www", Image, "project", quality, w, h);
                    }
                }
                _uow.Project.Update(entity);
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _uow.Project.GetById(Convert.ToInt32(id));
            if (item != null)
            {
                var nottype = _uow.NotificationType.GetNTByType("deleted-project");
                string notificationTitle = item.Title + " " + nottype.Message;

                #region deleteMedia
                _uow.Media.DeleteImage(item.Image, "www", "\\img\\project\\");
                #endregion
                _uow.Project.Delete(item);
                return RedirectToAction("List");
            }
            return View(item);
        }
    }
}
