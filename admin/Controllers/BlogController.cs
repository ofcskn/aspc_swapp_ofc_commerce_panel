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
    public class BlogController : Controller
    {
        private readonly IUnitOfWork _uow;
        public BlogController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpPost]
        public JsonResult PublishStatus(bool enabled, int itemId)
        {
            var item = _uow.Blog.GetById(itemId);
            if (item != null)
            {
                item.Enabled = enabled;
                _uow.Blog.Update(item);
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
            var list = _uow.Blog.GetAll();//Your list
            return View(await PaginatedList<Blog>.CreateAsync(list, pageNumber, 10));
        }
        [HttpGet]
        public IActionResult Manage(int? id)
        {
            if (id != 0)
            {
                return View(_uow.Blog.GetById(Convert.ToInt32(id)));
            }
            else
            {
                return View(new Blog());
            }
        }
        [HttpPost]
        public IActionResult DraftSave(Blog entity)
        {
            var blog = _uow.Blog.GetById(entity.Id);
            blog.Title = entity.Title;
            blog.SubTitle = entity.SubTitle;
            blog.Description = entity.Description;

            _uow.Blog.Update(blog);
            return Json(entity);
        }
        [HttpPost]
        public IActionResult Manage(Blog entity, IFormFile Image)
        {
            string filePath = "/img/blog/";
            int quality = 60;
            int w = 1920;
            int h = 1080;
            if (entity.Id == 0)
            {
                if (Image != null)
                {
                    entity.Image = _uow.Media.ChangeFileName(entity.Title, Image);
                    _uow.Media.InsertWithIFormFile(filePath, entity.Title, "www", Image, "blog", quality, w, h);
                    entity.Date = DateTime.Now;
                    entity.Permalink = _uow.Blog.GenerateAndInsertPermalink(entity.Title);
                    _uow.Blog.Add(entity);
                    return RedirectToAction("List");
                }
                else
                {
                    ViewBag.AlertMessage = "Lütfen fotoğraf ekleyiniz.";
                    return View(entity);
                }
            }
            else
            {
                if (Image != null)
                {
                    if (_uow.Media.ChangeFileName(Image.FileName, Image) != entity.Image)
                    {
                        _uow.Media.DeleteImage(entity.Image, "www", filePath);
                        entity.Image = _uow.Media.ChangeFileName(entity.Title, Image);
                        _uow.Media.InsertWithIFormFile(filePath, entity.Title, "www", Image, "blog", quality, w, h);
                    }
                }
                var blog = _uow.Blog.GetById(entity.Id);
                blog.Title = entity.Title;
                blog.SubTitle = entity.SubTitle;
                blog.Description = entity.Description;

                _uow.Blog.Update(blog);
                return RedirectToAction("List");
            }
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _uow.Blog.GetById(Convert.ToInt32(id));
            if (item != null)
            {
                #region deleteMedia
                _uow.Media.DeleteImage(item.Image, "www", "\\img\\blog\\");
                #endregion
                _uow.Blog.Delete(item);
                return RedirectToAction("List");
            }
            return View(item);
        }
    }
}
