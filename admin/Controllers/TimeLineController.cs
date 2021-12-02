using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.Context;
using Entity.Models;
using Entity.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using admin.ViewModel;
using admin.Models;

namespace admin.Controllers
{
    [Authorize]
    public class TimeLineController : Controller
    {
        private readonly IUnitOfWork _uow;
        public TimeLineController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public ActionResult Index(int? pageNumber)
        {
            TimelineViewModel vm = new TimelineViewModel();
            int pageSize = 4;//Bir sayfada kaç adet listeleme yapılacak.

            var newList = new List<TimelineModel>();
            foreach (var item in _uow.TimeLine.GetAllTL().Where(p => p.IsAll == true).OrderByDescending(p => p.Date))
            {
                var userId = item.MemberId;
                var userRole = item.MemberRole;

                TimelineModel avm = new TimelineModel();
                if (userRole == "admin" || userRole == "user")
                {
                    avm.MemberNameSurname = _uow.Admin.GetById(userId).Name + " " + _uow.Admin.GetById(userId).Surname;
                    avm.Avatar = _uow.Admin.GetById(userId).Image;
                }
                if (userRole == "current")
                {
                    avm.MemberNameSurname = _uow.Current.GetById(userId).Name + " " + _uow.Current.GetById(userId).Surname;
                    avm.Avatar = _uow.Current.GetById(userId).Image;
                }
                if (userRole == "staff")
                {
                    avm.MemberNameSurname = _uow.Staff.GetById(userId).Name + " " + _uow.Staff.GetById(userId).Surname;
                    avm.Avatar = _uow.Staff.GetById(userId).Image;
                }
                avm.Id = item.Id;
                avm.MemberId = item.MemberId;
                avm.MemberRole = item.MemberRole;
                avm.ColorCode = item.Tlekstra.ColorCode;
                avm.IconClass = item.Tlekstra.IconClass;
                avm.Title = item.Title;
                avm.Description = item.Description;
                avm.Date = Convert.ToDateTime(item.Date);
                newList.Add(avm);
            }
            if (pageNumber == null)
            {
                vm.Timelines = newList.Take(pageSize).ToList();
                return View(vm);
            }
            else
            {
                vm.Timelines = newList.Skip(pageSize * pageNumber.Value).Take(pageSize).ToList();
                return Json(vm);
            }
        }
        [HttpPost]
        public JsonResult Get(int id)
        {
            TimelineViewModel vm = new TimelineViewModel();
            vm.Timeline = _uow.TimeLine.GetById(id);
            return Json(vm);
        }
        [HttpPost]
        public IActionResult Manage(TimelineViewModel vm, int TimeLine_IsAll)
        {
            var entity = vm.Timeline;
            if (entity.Id == 0)
            {
                entity.IsAll = Convert.ToBoolean(TimeLine_IsAll);
                entity.Date = DateTime.Now;
                entity.Lang = "tr";
                entity.MemberId = Convert.ToInt32(_uow.Admin.GetIdByAdmin);
                entity.MemberRole = _uow.Admin.GetRoleByAdmin;
                _uow.TimeLine.Add(entity);
                return RedirectToAction("Index");
            }
            else
            {
                var timeline = _uow.TimeLine.GetById(entity.Id);
                timeline.IsAll = Convert.ToBoolean(TimeLine_IsAll);
                timeline.Title = entity.Title;
                timeline.Description = entity.Description;
                timeline.TlekstraId = entity.TlekstraId;
                _uow.TimeLine.Update(timeline);
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult TlExtra(TimelineViewModel vm)
        {
            var entity = vm.TimeLineEkstra;
            if (!entity.ColorCode.Contains("#"))
            {
                entity.ColorCode = "#" + entity.ColorCode;
            }
            _uow.TimeLine.AddTL(entity);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var item = _uow.TimeLine.GetById(id);
            _uow.TimeLine.Delete(item);
            return Json("ok");
        }
    }
}
