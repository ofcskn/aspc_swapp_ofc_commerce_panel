using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using admin.Models;
using Service.Abstract;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using admin.ViewModel;

namespace admin.Controllers
{
    [Authorize]
    public class ToDoListController : Controller
    {
        private readonly IUnitOfWork _uow;
        public ToDoListController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<IActionResult> List(string filter, int? groupid, int pageNumber = 1)
        {
            if (groupid != null)
            {
                ToDoListViewModel vm = new ToDoListViewModel();
                var list = _uow.ToDoList.GetAllByGroupId(Convert.ToInt32(groupid), false);
                if (filter == "h")
                {
                    list = _uow.ToDoList.GetAllByGroupId(Convert.ToInt32(groupid), true);
                }
                vm.ToDoListGroup = _uow.ToDoListGroup.GetByIdWithInclude(Convert.ToInt32(groupid));
                vm.PToDoLists = await PaginatedList<ToDoList>.CreateAsync(list, pageNumber, 20);
                return View(vm);
            }
            else
            {
                ToDoListViewModel vm = new ToDoListViewModel();
                if (filter == "h")
                {
                    var userId = _uow.Admin.GetIdByAdmin;
                    var userRole = _uow.Admin.GetRoleByAdmin;
                    //Get todolist by enabled
                    var list = _uow.ToDoList.GetAllByUser(userId, userRole, true).Where(p=>p.GroupId == null);
                    vm.PToDoLists = await PaginatedList<ToDoList>.CreateAsync(list, pageNumber, 20);
                    return View(vm);
                }
                else
                {
                    var userId = _uow.Admin.GetIdByAdmin;
                    var userRole = _uow.Admin.GetRoleByAdmin;
                    var list = _uow.ToDoList.GetAllByUser(userId, userRole, false).Where(p => p.GroupId == null);
                    vm.PToDoLists = await PaginatedList<ToDoList>.CreateAsync(list, pageNumber, 20);
                    return View(vm);
                }
            }
        }
        [HttpPost]
        public JsonResult Check(int id)
        {
            var goal = _uow.ToDoList.GetById(id);
            if (goal.Enabled == false)
            {
                goal.Enabled = true;
                goal.EndDate = DateTime.Now;
            }
            else
            {
                goal.Enabled = false;
            }
            _uow.ToDoList.Update(goal);
            return Json(goal);
        }
        [HttpPost]
        public JsonResult Add(string todo, int? groupId)
        {
            if (todo != null)
            {
                ToDoList entity = new ToDoList
                {
                    Goal = todo,
                    StartDate = DateTime.Now,
                    Enabled = false,
                    MemberId = Convert.ToInt32(_uow.Admin.GetIdByAdmin),
                    Role = _uow.Admin.GetRoleByAdmin,
                };
                if (groupId != null)
                {
                    entity.GroupId = groupId;
                }
                _uow.ToDoList.Add(entity);
                if (_uow.ToDoList.GetAllByEnabled(_uow.Admin.GetIdByAdmin, _uow.Admin.GetRoleByAdmin, false).Count() == 20)
                {
                    //Send To Do List Notification To The Person
                    var nottype = _uow.NotificationType.GetNTByType("not-completed-20-goals");
                    if (_uow.ToDoList.GetAllByEnabled(_uow.Admin.GetIdByAdmin, _uow.Admin.GetRoleByAdmin, false).Count() == 35)
                    {
                        nottype = _uow.NotificationType.GetNTByType("not-completed-too-goals");
                    }
                    string notificationTitle = nottype.Message;
                    try
                    {
                        //Add Notification
                        Notification notification = new Notification
                        {
                            Enabled = false,
                            NotTypeId = nottype.Id,
                            SendDate = DateTime.Now,
                            Title = notificationTitle,
                            UserId = _uow.Admin.GetIdByAdmin,
                            UserRole = _uow.Admin.GetRoleByAdmin
                        };
                        _uow.Notification.Add(notification);
                    }
                    catch (Exception)
                    {
                    }
                }
                return Json(entity);
            }
            return Json("no");
        }
        [HttpPost]
        public JsonResult Update(int id, string goal)
        {
            var todo = _uow.ToDoList.GetById(id);
            todo.Goal = goal;
            _uow.ToDoList.Update(todo);
            return Json(todo);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var goal = _uow.ToDoList.GetById(id);
            if (goal != null)
            {
                _uow.ToDoList.Delete(goal);
                return Json(goal);
            }
            return Json("no");
        }
    }
}
