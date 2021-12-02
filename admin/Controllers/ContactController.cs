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
    public class ContactController : Controller
    {
        private readonly IUnitOfWork _uow;
        public ContactController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [HttpGet]
        public async Task<IActionResult> List(int pageNumber = 1)
        {
            var list = _uow.Contact.GetAll();//Your list
            return View(await PaginatedList<Contact>.CreateAsync(list, pageNumber, 10));
        }

        public IActionResult Delete(int id)
        {
            var item = _uow.Contact.GetById(Convert.ToInt32(id));
            if (item != null)
            {
                _uow.Contact.Delete(item);
                return Json("success");
            }
            return Json("failed");
        }
    }
}
