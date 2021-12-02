using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using admin.Models;
using admin.ViewModel;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using Service.Utilities;

namespace admin.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly IUnitOfWork _uow;
        public SearchController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [Authorize(Policy = "Staff")]
        [HttpGet]
        public async Task<IActionResult> List(int pageNumber = 1)
        {
            var list = _uow.Search.GetAll();//Your list
            return View(await PaginatedList<Search>.CreateAsync(list, pageNumber, 20));
        }
        [Authorize(Policy = "User")]
        [HttpGet]
        public IActionResult Manage(int? id)
        {
            if (id != 0)
            {
                return View(_uow.Search.GetById(Convert.ToInt32(id)));
            }
            else
            {
                return View(new Search());
            }
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult Manage(Search entity)
        {
            if (entity.Id == 0)
            {
                entity.Date = DateTime.Now;
                entity.Enabled = true;
                _uow.Search.Add(entity);
            }
            else
            {
                _uow.Search.Update(entity);
            }
            return RedirectToAction("List");
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _uow.Search.GetById(Convert.ToInt32(id));
            if (item != null)
            {
                _uow.Search.Delete(item);
                return RedirectToAction("List");
            }
            return View(item);
        }
        [HttpGet]
        public IActionResult Result(string q)
        {
            SearchViewModel svm = new SearchViewModel();
            svm.Products = _uow.Product.GetAllPCByFilter(q);
            if (q != null)
            {
                svm.SearchResults = _uow.Search.GetAllByFilter(q);
                return View(svm);
            }
            else
            {
                svm.SearchResults = _uow.Search.GetAllByEnabled(true);
                svm.Products = _uow.Product.GetAllPC().ToList();
                return View(svm);
            }
        }

    }
}
