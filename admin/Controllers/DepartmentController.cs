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
using admin.Models;

namespace admin.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _uow;
        public DepartmentController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [Authorize(Policy = "User")]
        [HttpPost]
        public JsonResult PublishStatus(bool enabled, int departmentId)
        {
            var item = _uow.Department.GetById(departmentId);
            if (item != null)
            {
                item.Status = enabled;
                _uow.Department.Update(item);
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
            var list = _uow.Department.GetAll();//Your list
            return View(await PaginatedList<Department>.CreateAsync(list, pageNumber, 10));
        }
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult Manage(Department entity)
        {
            if (ModelState.IsValid)
            {
                if (entity.Id == 0)
                {
                    entity.Status = false;
                    _uow.Department.Add(entity);

                    //Department Notification To Everyone
                    var nottype = _uow.NotificationType.GetNTByType("new-department");
                    string notificationTitle = entity.Name + " " + nottype.Message;
                    _uow.Notification.AddListForAdmin(nottype.Id, notificationTitle, _uow.Admin.GetAllByEnabled(true));
                    _uow.Notification.AddListForStaff(nottype.Id, notificationTitle, _uow.Staff.GetAllByEnabled());
                    _uow.Notification.AddListForCurrent(nottype.Id, notificationTitle, _uow.Current.GetAllByEnabled(true));

                    return Json("ok");
                }
                else
                {
                    entity.Status = false;
                    _uow.Department.Update(entity);
                    return Json("ok");
                }
            }
            else
            {
                return Json("fail");
            }
        }
        public IActionResult Staff(int id)
        {
            var staff = _uow.Staff.GetAllByDepartment(id);
            return View(staff);
        }
        [Authorize(Policy = "Admin")]
        public IActionResult Delete(int id)
        {
            var item = _uow.Department.GetById(id);
            _uow.Department.Delete(item);

            //Department Notification To Everyone
            var nottype = _uow.NotificationType.GetNTByType("deleted-department");
            string notificationTitle = item.Name + " " + nottype.Message;
            _uow.Notification.AddListForAdmin(nottype.Id, notificationTitle, _uow.Admin.GetAllByEnabled(true));
            _uow.Notification.AddListForStaff(nottype.Id, notificationTitle, _uow.Staff.GetAllByEnabled());
            _uow.Notification.AddListForCurrent(nottype.Id, notificationTitle, _uow.Current.GetAllByEnabled(true));

            return Json("ok");
        }
    }
}
