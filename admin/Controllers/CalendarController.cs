using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using admin.Models;
using admin.ViewModel;
using Entity.Context;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;

namespace admin.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly IUnitOfWork _uow;
        public CalendarController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public IActionResult Index()
        {
            CalendarViewModel cvm = new CalendarViewModel();
            return View(cvm);
        }
        public JsonResult Manage(Calendar entity)
        {
            if (entity.Id == 0)
            {
                entity.UserId = _uow.Admin.GetIdByAdmin;
                entity.UserRole = _uow.Admin.GetRoleByAdmin;
                _uow.Calendar.Add(entity);
                return Json(entity);
            }
            else
            {
                var events = _uow.Calendar.GetById(entity.Id);
                events.Title = entity.Title;
                events.Subject = entity.Subject;
                events.Description = entity.Description;
                events.Type = entity.Type;
                events.BorderColor = entity.BorderColor;
                events.BackgroundColor = entity.BackgroundColor;
                events.Status = entity.Status;
                events.AllDay = entity.AllDay;
                _uow.Calendar.Update(events);
                return Json(entity);
            }
        }
        [HttpPost]
        public JsonResult AddEvent(Calendar entity)
        {
            entity.UserId = _uow.Admin.GetIdByAdmin;
            entity.UserRole = _uow.Admin.GetRoleByAdmin;
            entity.StartDate = DateTime.Now;
            _uow.Calendar.Add(entity);
            return Json(entity);
        }
        public JsonResult UpdateEvent(int id, DateTime start)
        {
            var events = _uow.Calendar.GetById(id);
            events.StartDate = start;
            _uow.Calendar.Update(events);
            return Json("success");
        }
        public JsonResult UpdateStartDate(int id, string start)
        {
            var events = _uow.Calendar.GetById(id);
            events.StartDate = Convert.ToDateTime(start);
            _uow.Calendar.Update(events);
            return Json("success");
        }
        public JsonResult Get(int id)
        {
            var events = _uow.Calendar.GetById(id);
            return Json(events);
        }
        public JsonResult UpdateEndEvent(int id, DateTime end)
        {
            var events = _uow.Calendar.GetById(id);
            events.EndDate = end;
            _uow.Calendar.Update(events);
            return Json("success");
        }
        public IActionResult FindAllEvents()
        {
            return new JsonResult(_uow.Calendar.FindAllEvents(_uow.Admin.GetIdByAdmin, _uow.Admin.GetRoleByAdmin));
        }
    }
}
