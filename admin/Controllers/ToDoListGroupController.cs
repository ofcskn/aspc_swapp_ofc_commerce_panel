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
    public class ToDoListGroupController : Controller
    {
        private readonly IUnitOfWork _uow;
        public ToDoListGroupController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<IActionResult> List(int groupid, string filter, int pageNumber = 1)
        {
            ToDoListViewModel vm = new ToDoListViewModel();
            if (_uow.ToDoListGroup.GetAll().Any(p => p.GroupId == groupid))
            {
                vm.PToDoListGroups = await PaginatedList<ToDoListGroup>.CreateAsync(_uow.ToDoListGroup.GetAllByGroupId(true, groupid), pageNumber, 20);
                vm.ToDoListGroups = _uow.ToDoListGroup.GetAll().Where( p => p.GroupId == groupid);
                return View(vm);
            }
            else
            {
                if (groupid != 0)
                {
                    return Redirect("/admin/todolist/list?groupid=" + groupid);
                }
                else
                {
                    var userId = _uow.Admin.GetIdByAdmin;
                    var userRole = _uow.Admin.GetRoleByAdmin;
                    var groups = _uow.ToDoListGroup.GetAllByUser(userId, userRole, true);
                    vm.PToDoListGroups = await PaginatedList<ToDoListGroup>.CreateAsync(groups, pageNumber, 20);
                    vm.ToDoListGroups = _uow.ToDoListGroup.GetAll();
                    return View(vm);
                }
            }
        }
        public IActionResult Detail(int id)
        {
            var group = _uow.ToDoListGroup.GetByIdWithInclude(id);
            ToDoListViewModel vm = new ToDoListViewModel();
            vm.ToDoListGroup = group;
            vm.ToDoListGroups = _uow.ToDoListGroup.GetAllByGroupId(true, id);

            var newList = new List<UserViewModel>();
            foreach (var item in _uow.ToDoListUser.GetAllByGroupId(id))
            {
                UserViewModel userViewModel = new UserViewModel();
                userViewModel.FullName = _uow.Admin.GetUserNameSurname(item.UserRole, item.UserId, "name");
                userViewModel.Username = _uow.Admin.GetUserNameSurname(item.UserRole, item.UserId, "username");
                userViewModel.Id = item.UserId;
                userViewModel.Role = item.UserRole;
                userViewModel.Avatar = _uow.Admin.GetUserImage(item.UserRole, item.UserId);
                userViewModel.AvatarSrc = item.UserRole;
                userViewModel.Enabled = Convert.ToBoolean(item.Enabled);
                newList.Add(userViewModel);
            }
            vm.Users = newList.AsQueryable();
            vm.AdminName = _uow.Admin.GetUserNameSurname(group.AdminRole, group.AdminId, "name");
            return View(vm);
        }
        [HttpPost]
        public JsonResult AddUserToGroup(int userId, string userRole, int groupId)
        {
            if (groupId != 0 && userRole != null && groupId != 0)
            {
                if (!_uow.ToDoListUser.GetAll().Any(p => p.UserId == userId && p.UserRole == userRole && p.GroupId == groupId))
                {
                    ToDoListUser user = new ToDoListUser
                    {
                        UserId = userId,
                        UserRole = userRole,
                        Date = DateTime.Now,
                        Enabled = true,
                        GroupId = groupId,
                    };
                    _uow.ToDoListUser.Add(user);

                    var nottype = _uow.NotificationType.GetNTByType("added-user-in-todogroup");
                    try
                    {
                        //Add Notification
                        Notification notification = new Notification
                        {
                            Enabled = false,
                            NotTypeId = nottype.Id,
                            SendDate = DateTime.Now,
                            Title = _uow.Admin.GetUserNameSurname(_uow.Admin.GetRoleByAdmin, _uow.Admin.GetIdByAdmin, "name") + " " + nottype.Message,
                            UserId = Convert.ToInt32(userId),
                            UserRole = userRole
                        };
                        _uow.Notification.Add(notification);
                    }
                    catch (Exception)
                    {
                    }
                    return Json("yes");
                }
                else
                {
                    return Json("found-user");
                }

            }
            return Json("no");
        }
        [HttpPost]
        public JsonResult GetUsers(string filter, int? pageNumber, int groupId)
        {
            int pageSize = 8;//Bir sayfada kaç adet listeleme yapılacak.
            var newList = new List<UserViewModel>();
            var todoUserList = _uow.ToDoListUser.GetAll();

            if (filter == "admin")
            {
                IQueryable<Admin> list = _uow.Admin.GetAllByEnabled(true).Take(pageSize);
                if (pageNumber != 0)
                {
                    list = _uow.Admin.GetAllByEnabled(true).Skip(pageSize * pageNumber.Value).Take(pageSize);
                }
                foreach (var item in list)
                {
                    if (!todoUserList.Any(p => p.UserId == item.Id && p.UserRole == "admin" && p.GroupId == groupId))
                    {
                        UserViewModel userViewModel = new UserViewModel();
                        userViewModel.FullName = _uow.Admin.GetUserNameSurname("admin", item.Id, "name");
                        userViewModel.Username = _uow.Admin.GetUserNameSurname("admin", item.Id, "username");
                        userViewModel.Id = item.Id;
                        userViewModel.Role = "admin";
                        userViewModel.Avatar = _uow.Admin.GetUserImage("admin", item.Id);
                        userViewModel.AvatarSrc = "admin";
                        userViewModel.Enabled = Convert.ToBoolean(item.Enabled);
                        newList.Add(userViewModel);
                    }
                }
                return Json(newList);
            }
            if (filter == "staff")
            {
                IQueryable<Staff> list = _uow.Staff.GetAllByEnabled().Take(pageSize);
                if (pageNumber != 0)
                {
                    list = _uow.Staff.GetAllByEnabled().Skip(pageSize * pageNumber.Value).Take(pageSize);
                }
                foreach (var item in list)
                {
                    if (!todoUserList.Any(p => p.UserId == item.Id && p.UserRole == "staff" && p.GroupId == groupId))
                    {
                        UserViewModel userViewModel = new UserViewModel();
                        userViewModel.FullName = _uow.Admin.GetUserNameSurname("staff", item.Id, "name");
                        userViewModel.Username = _uow.Admin.GetUserNameSurname("staff", item.Id, "username");
                        userViewModel.Id = item.Id;
                        userViewModel.Role = "staff";
                        userViewModel.Avatar = _uow.Admin.GetUserImage("staff", item.Id);
                        userViewModel.AvatarSrc = "staff";
                        newList.Add(userViewModel);
                    }
                }
                return Json(newList);
            }
            if (filter == "added-users")
            {
                var g = _uow.ToDoListGroup.GetById(groupId);
                var list = _uow.ToDoListUser.GetAll().Where(p => p.GroupId == groupId && !(g.AdminId == p.UserId && g.AdminRole == p.UserRole)).Take(pageSize);
                if (pageNumber != 0)
                {
                    list = _uow.ToDoListUser.GetAll().Where(p => p.GroupId == groupId && !(g.AdminId == p.UserId && g.AdminRole == p.UserRole)).Skip(pageSize * pageNumber.Value).Take(pageSize);
                }
                foreach (var item in list)
                {
                    UserViewModel userViewModel = new UserViewModel();
                    userViewModel.FullName = _uow.Admin.GetUserNameSurname(item.UserRole, item.UserId, "name");
                    userViewModel.Username = _uow.Admin.GetUserNameSurname(item.UserRole, item.UserId, "username");
                    userViewModel.Id = item.UserId;
                    userViewModel.Role = item.UserRole;
                    userViewModel.Avatar = _uow.Admin.GetUserImage(item.UserRole, item.UserId);
                    userViewModel.AvatarSrc = item.UserRole;
                    newList.Add(userViewModel);
                }
                return Json(newList);
            }
            return Json("no-data");
        }
        public IActionResult GroupUser(int groupId)
        {
            ToDoListViewModel vm = new ToDoListViewModel
            {
                ToDoListGroup = _uow.ToDoListGroup.GetById(groupId)
            };
            return View(vm);
        }
        [HttpGet]
        public IActionResult Manage(int? id, int? groupId)
        {
            ToDoListViewModel vm = new ToDoListViewModel();
            vm.GroupId = groupId.ToString();
            if (id != null)
            {
                vm.ToDoListGroup = _uow.ToDoListGroup.GetById(Convert.ToInt32(id));
                return View(vm);
            }
            else
            {
                vm.ToDoListGroup = new ToDoListGroup();
                return View(vm);
            }
        }
        [HttpPost]
        public IActionResult Manage(ToDoListViewModel vm, IFormFile Image, IFormFile Icon)
        {
            string filePath = "/img/todolistgroup/";
            int quality = 80;
            int w = 1920;
            int h = 384;
            int wicon = 50;
            int hicon = 50;
            var entity = vm.ToDoListGroup;
            vm.ToDoListGroup = entity;
            vm.GroupId = entity.GroupId.ToString();
            if (entity.Id == 0)
            {
                if (ModelState.IsValid)
                {
                    if (Image != null)
                    {
                        entity.Image = _uow.Media.ChangeFileName(entity.Title, Image);
                        _uow.Media.InsertWithIFormFile(filePath, entity.Title, "admin", Image, "todolistgroup", quality, w, h);
                    }
                    if (Icon != null)
                    {
                        entity.Icon = _uow.Media.ChangeFileName(entity.Title, Icon);
                        _uow.Media.InsertWithIFormFile(filePath, entity.Title, "admin", Icon, "todolistgroup", quality, wicon, hicon);
                    }

                    entity.Date = DateTime.Now;
                    entity.Enabled = false;
                    entity.AdminId = _uow.Admin.GetIdByAdmin;
                    entity.AdminRole = _uow.Admin.GetRoleByAdmin;
                    entity.Priority = _uow.ToDoListGroup.GetAll().Max(p => p.Priority) + 1;
                    _uow.ToDoListGroup.Add(entity);

                    var nottype = _uow.NotificationType.GetNTByType("new-todolist-group");
                    string notificationTitle = entity.Title.ToUpper() + " " + nottype.Message;
                    try
                    {
                        _uow.Notification.AddListForStaff(nottype.Id, notificationTitle, _uow.Staff.GetAllByEnabled());
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        _uow.Notification.AddListForAdmin(nottype.Id, notificationTitle, _uow.Admin.GetAllByEnabled(true));
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    return View(vm);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (Image != null)
                    {
                        entity.GroupId = entity.GroupId;
                        if (_uow.Media.ChangeFileName(Image.FileName, Image) != entity.Image)
                        {
                            _uow.Media.DeleteImage(entity.Image, "admin", filePath);
                            entity.Image = _uow.Media.ChangeFileName(entity.Title, Image);
                            _uow.Media.InsertWithIFormFile(filePath, entity.Title, "admin", Image, "todolistgroup", quality, w, h);
                        }
                    }
                    if (Icon != null)
                    {
                        if (_uow.Media.ChangeFileName(Icon.FileName, Icon) != entity.Icon)
                        {
                            _uow.Media.DeleteImage(entity.Icon, "admin", filePath);
                            entity.Icon = _uow.Media.ChangeFileName(entity.Title, Icon);
                            _uow.Media.InsertWithIFormFile(filePath, entity.Title, "admin", Icon, "todolistgroup", quality, wicon, hicon);
                        }
                    }
                    _uow.ToDoListGroup.Update(entity);
                }
                else
                {
                    return View(vm);
                }
            }
            return Redirect("/admin/todolist/list?groupid=" + entity.Id);
        }
    [Authorize(Policy = "Admin")]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var item = _uow.ToDoListGroup.GetById(Convert.ToInt32(id));
            if (item != null)
            {
                if (item.AdminId == _uow.Admin.GetIdByAdmin && item.AdminRole == _uow.Admin.GetRoleByAdmin)
                {
                    var nottype = _uow.NotificationType.GetNTByType("deleted-todolist-group");
                    string notificationTitle = item.Title + " " + nottype.Message;
                    try
                    {
                        _uow.Notification.AddListForStaff(nottype.Id, notificationTitle, _uow.Staff.GetAllByEnabled());
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        _uow.Notification.AddListForAdmin(nottype.Id, notificationTitle, _uow.Admin.GetAllByEnabled(true));
                    }
                    catch (Exception)
                    {
                    }

                    #region deleteMedia
                    _uow.Media.DeleteImage(item.Image, "admin", "\\img\\todolistgroup\\");
                    _uow.Media.DeleteImage(item.Icon, "admin", "\\img\\todolistgroup\\");
                    #endregion
                    _uow.ToDoListGroup.Delete(item);
                    return Json("ok");
                }
                else
                {
                    return Json("no-admin");
                }
            }
            return Json("no");
        }
    }
}
