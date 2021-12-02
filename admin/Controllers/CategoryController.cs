using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using admin.Models;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;

namespace admin.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _uow;
        public CategoryController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<IActionResult> List(int pageNumber = 1)
        {
            return View(await PaginatedList<Category>.CreateAsync(_uow.Category.GetAll().Include(p => p.Product), pageNumber, 10));
        }
        [Authorize(Policy = "User")]
        [HttpPost]
        public JsonResult PublishStatus(bool enabled, int categoryId)
        {
            var page = _uow.Category.GetById(categoryId);
            if (page != null)
            {
                //page.Status = enabled;
                _uow.Category.Update(page);
                if (enabled == true)
                {
                    return Json("start");
                }
                return Json("pause");
            }
            return Json("no");
        }
        [Authorize(Policy = "User")]
        [HttpPost]
        public IActionResult Manage(Category entity)
        {
            if (ModelState.IsValid)
            {
                if (entity.Id != 0)
                {
                    _uow.Category.Update(entity);
                    return Json("ok");
                }
                else
                {
                    _uow.Category.Add(entity);
                    return Json("ok");
                }
            }
            else
            {
                return Json("fail");
            }
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = _uow.Category.GetById(Convert.ToInt32(id));
            return View(category);
        }
        [Authorize(Policy = "Admin")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _uow.Category.GetById(Convert.ToInt32(id));
            if (category != null)
            {
                _uow.Category.Delete(category);
                return RedirectToAction("List");
            }
            return View(category);
        }
    }
}
