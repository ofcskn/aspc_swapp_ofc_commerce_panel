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
    public class CargoProcessController : Controller
    {
        private readonly IUnitOfWork _uow;
        public CargoProcessController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public IActionResult Status(int cargoid)
        {
            ViewBag.ProductCargoId = cargoid;
            return View(_uow.CargoProcess.GetAllByCargo(cargoid).Include(p => p.ProductCargo));
        }
        [Authorize(Policy = "Staff")]
        [HttpPost]
        public JsonResult PublishStatus(bool enabled, int itemId)
        {
            var item = _uow.CargoProcess.GetById(itemId);
            if (item != null)
            {
                item.Enabled = enabled;
                _uow.CargoProcess.Update(item);
                if (enabled == true)
                {
                    return Json("start");
                }
                return Json("pause");
            }
            return Json("no");
        }
        [HttpPost]
        public JsonResult Get(int id)
        {
            var cargoProcess = _uow.CargoProcess.GetById(id);
            return Json(cargoProcess);
        }
        [Authorize(Policy = "Staff")]
        [HttpPost]
        public IActionResult Manage(CargoProcess entity)
        {
            if (ModelState.IsValid)
            {
                if (entity.Id != 0)
                {
                    var cargoProcess = _uow.CargoProcess.GetById(entity.Id);
                    cargoProcess.Title = entity.Title;
                    cargoProcess.Description = entity.Description;
                    _uow.CargoProcess.Update(cargoProcess);
                    return Json("success");
                }
                else
                {
                    entity.Date = DateTime.Now;
                    entity.Enabled = false;
                    _uow.CargoProcess.Add(entity);
                    return Json("success");
                }
            }
            else
            {
                return Json("fail");
            }
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var item = _uow.CargoProcess.GetById(Convert.ToInt32(id));
            if (item != null)
            {
                _uow.CargoProcess.Delete(item);
                return Json("success");
            }
            return Json("error");
        }
    }
}
