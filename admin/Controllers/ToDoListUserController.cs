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
using Microsoft.AspNetCore.Http;

namespace admin.Controllers
{
    [Authorize]
    public class ToDoListUserController : Controller
    {
        private readonly IUnitOfWork _uow;
        public ToDoListUserController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult Delete(int userId, string userRole, int groupId)
        {
            var item = _uow.ToDoListUser.GetAll().FirstOrDefault(p => p.UserId == userId && p.UserRole == userRole && p.GroupId == groupId);
            if (item != null)
            {
                var group = _uow.ToDoListGroup.GetById(Convert.ToInt32(item.GroupId));
                _uow.ToDoListUser.Delete(item);
                var nottype = _uow.NotificationType.GetNTByType("deleted-user-in-todogroup");
                try
                {
                    //Add Notification
                    Notification notification = new Notification
                    {
                        Enabled = false,
                        NotTypeId = nottype.Id,
                        SendDate = DateTime.Now,
                        Title = group.Title + " " + nottype.Message,
                        UserId = Convert.ToInt32(item.UserId),
                        UserRole = item.UserRole
                    };
                    _uow.Notification.Add(notification);
                }
                catch (Exception)
                {
                }
                return Json("ok");
            }
            return Json("no");
        }
    }
}
