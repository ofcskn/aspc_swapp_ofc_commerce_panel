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
using Service.Utilities;
using Microsoft.AspNetCore.Http;
using admin.Models;

namespace admin.Controllers
{
    [Authorize]
    public class StaffController : Controller
    {
        private readonly IUnitOfWork _uow;
        public StaffController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public JsonResult PublishStatus(bool enabled, int staffId)
        {
            var item = _uow.Staff.GetById(staffId);
            if (item != null)
            {
                item.Status = enabled;
                _uow.Staff.Update(item);
                if (enabled == true)
                {
                    // Notification To Admins and Staffs
                    var nottype = _uow.NotificationType.GetNTByType("started-staff");
                    string notificationTitle = item.Name + " " + item.Surname + " " + nottype.Message;
                    try
                    {
                        _uow.Notification.AddListForAdmin(nottype.Id, notificationTitle, _uow.Admin.GetAllByEnabled(true));
                        _uow.Notification.AddListForStaff(nottype.Id, notificationTitle, _uow.Staff.GetAllByEnabled());
                    }
                    catch (Exception)
                    {
                    }
                    return Json("start");
                }
                else
                {
                    // Notification To Admins and Staffs
                    var nottype = _uow.NotificationType.GetNTByType("paused-staff");
                    string notificationTitle = item.Name + " " + item.Surname + " " + nottype.Message;
                    try
                    {
                        _uow.Notification.AddListForAdmin(nottype.Id, notificationTitle, _uow.Admin.GetAllByEnabled(true));
                        _uow.Notification.AddListForStaff(nottype.Id, notificationTitle, _uow.Staff.GetAllByEnabled());
                    }
                    catch (Exception)
                    {
                    }
                }
                return Json("pause");
            }
            return Json("no");
        }
        [Authorize(Policy = "User")]
        [HttpGet]
        public async Task<IActionResult> List(int? departmentid, int pageNumber = 1)
        {
            if (departmentid != null)
            {
                var list = _uow.Staff.GetAllByDepartment(Convert.ToInt32(departmentid));//Your list
                return View(await PaginatedList<Staff>.CreateAsync(list, pageNumber, 20));
            }
            else
            {
                var list = _uow.Staff.GetAllByDate();//Your list
                return View(await PaginatedList<Staff>.CreateAsync(list, pageNumber, 20));
            }
        }
        [HttpGet]
        public IActionResult Contacts(string filter, int? id)
        {
            if (filter == "department")
            {
                return View(_uow.Staff.GetAllByDepartment(Convert.ToInt32(id)));
            }
            else
            {
                return View(_uow.Staff.GetAllByDate());
            }
        }
        [Authorize(Policy = "Admin")]
        [HttpGet]
        public IActionResult Manage(int? id)
        {
            if (id != null)
            {
                var item = _uow.Staff.GetById(Convert.ToInt32(id));
                var decryptedPassword = Cipher.Decrypt(item.Password, item.UserName);
                item.Password = decryptedPassword;
                return View(item);
            }
            return View(new Staff());
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult Manage(Staff entity, IFormFile Image)
        {
            int quality = 60;
            int w = 500;
            int h = 500;
            //For small images
            int ws = 50;
            int hs = 50;
            string filePath = "/img/staff/";
            string fileName = entity.Name + " " + entity.Surname;
            if (entity.Id == 0)
            {
                if (ModelState.IsValid)
                {
                    string email = entity.Email.ToLower();

                    if (_uow.Admin.ControlUserEmail(email, "staff", entity.Id) == true && _uow.Admin.ControlUserName(entity.UserName, "staff", entity.Id) == true)
                    {
                        entity.LastLoginDate = DateTime.Now;
                        entity.LastLoginIp = _uow.Admin.GetIp();
                        entity.Image = _uow.Media.ChangeFileName(fileName, Image);
                        _uow.Media.InsertWithIFormFile(filePath, fileName, "admin", Image, "staff", quality, w, h);
                        _uow.Media.InsertWithIFormFile(filePath, fileName + "-s", "admin", Image, "staff", quality, ws, hs);

                        entity.UserName = entity.UserName.ToLower();
                        entity.Email = entity.Email.ToLower();
                        entity.Password = Cipher.Encrypt(entity.Password, entity.UserName);
                        entity.Status = false;
                        _uow.Staff.Add(entity);

                    }
                    else
                    {
                        if (_uow.Admin.ControlUserName(entity.UserName, "staff", entity.Id) == false && _uow.Admin.ControlUserEmail(email, "staff", entity.Id) == false)
                        {
                            ViewBag.ErrorMessage = "Bu kullanıcı adı ve email adresi sistemimizde kayıtlıdır.";
                        }
                        else if (_uow.Admin.ControlUserName(entity.UserName, "staff", entity.Id) == false)
                        {
                            ViewBag.ErrorMessage = "Bu kullanıcı adı sistemimizde kayıtlıdır. Lütfen farklı bir kullanıcı adı giriniz.";
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Bu email adresi sistemimizde kayıtlıdır. Lütfen farklı bir email adresi giriniz.";
                        }
                        return View(entity);
                    }
                }
                return View(entity);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    string email = entity.Email.ToLower();

                    entity.UserName = entity.UserName.ToLower();
                    entity.Email = email;
                    entity.Password = Cipher.Encrypt(entity.Password, entity.UserName);
                    if (Image != null)
                    {
                        if (_uow.Media.ChangeFileName(Image.FileName, Image) != entity.Image)
                        {
                            _uow.Media.DeleteImage(entity.Image, "admin", filePath);
                            _uow.Media.DeleteImage(entity.Image + "-s", "admin", filePath);
                            entity.Image = _uow.Media.ChangeFileName(fileName, Image);
                            _uow.Media.InsertWithIFormFile(filePath, fileName, "admin", Image, "staff", quality, w, h);
                            _uow.Media.InsertWithIFormFile(filePath, fileName + "-s", "admin", Image, "staff", quality, ws, hs);
                        }
                    }
                    _uow.Staff.Update(entity);
                    return RedirectToAction("List");
                }
                return View(entity);
            }
        }
        [Authorize(Policy = "Admin")]
        public IActionResult Delete(int id)
        {
            var item = _uow.Staff.GetById(id);
            if (item != null)
            {
                #region deleteMedia
                _uow.Media.DeleteImage(item.Image, "admin", "\\img\\staff\\");
                #endregion
                _uow.Staff.Delete(item);
                // Notification To Admins and Staffs
                var nottype = _uow.NotificationType.GetNTByType("deleted-staff");
                string notificationTitle = item.Name + " " + item.Surname + " " + nottype.Message;
                try
                {
                    _uow.Notification.AddListForAdmin(nottype.Id, notificationTitle, _uow.Admin.GetAllByEnabled(true));
                    _uow.Notification.AddListForStaff(nottype.Id, notificationTitle, _uow.Staff.GetAllByEnabled());
                }
                catch (Exception)
                {
                }
                return Json("ok");
            }
            return Json("fail");
        }
    }
}
