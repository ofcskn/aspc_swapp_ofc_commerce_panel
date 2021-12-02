using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using admin.Models;
using Service.Abstract;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Utilities;
using Microsoft.AspNetCore.Http;
using admin.ViewModel;

namespace admin.Controllers
{
    [Authorize]
    public class SettingController : Controller
    {
        private readonly IUnitOfWork _uow;
        public SettingController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public JsonResult GetDefaultSetting()
        {
            var settings_cookie = _uow.Cookie.GetCookie("settings_cookie");
            if (settings_cookie != null)
            {
                var splitted = _uow.Cookie.GetCookie("settings_cookie").Split(",");
                return Json(splitted);
            }
            return Json("fail");
        }
        public JsonResult SetDefaultSetting(int navbar_border, int font_size_page, int font_size_navbar, int font_size_sidebar, int font_size_footer, int sidebar_flat_style, int sidebar_legacy_style, int sidebar_compact, int sidebar_intend, int hover_focus_sidebar)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(60);
            string settings = "|" + navbar_border + "|" + font_size_page + "|" + font_size_navbar + "|" + font_size_sidebar + "|" + sidebar_flat_style + "|" + sidebar_legacy_style + "|" + sidebar_compact + "|" + sidebar_intend + "|" + hover_focus_sidebar;
            _uow.Cookie.SetCookie("settings_cookie", settings, option);
            return Json(settings);
        }
        [Authorize(Policy = "User")]
        [HttpGet]
        public IActionResult ProfileAdmin()
        {
            var id = _uow.Admin.GetIdByAdmin;
            Admin admin = _uow.Admin.GetById(id);
            var decryptedPassword = Cipher.Decrypt(admin.Password, admin.UserName);
            admin.Password = decryptedPassword;
            return View(admin);
        }

        [Authorize(Policy = "User")]
        [HttpPost]
        public IActionResult ProfileAdmin(Admin entity, IFormFile Image)
        {
            string filePath = "/img/admin/";
            int quality = 80;
            int w = 500;
            int h = 500;
            if (ModelState.IsValid)
            {
                string email = entity.Email.ToLower();
                var controlUserName = _uow.Admin.ControlUserName(entity.UserName, "admin", entity.Id);
                var controlUserEmail = _uow.Admin.ControlUserEmail(email, "admin", entity.Id);
                if (controlUserEmail == true && controlUserName == true)
                {
                    if (Image != null)
                    {
                        if (_uow.Media.ChangeFileName(Image.FileName, Image) != entity.Image)
                        {
                            _uow.Media.DeleteImage(entity.Image, "admin", filePath);
                            entity.Image = _uow.Media.ChangeFileName(entity.UserName, Image);
                            _uow.Media.InsertWithIFormFile(filePath, entity.UserName, "admin", Image, "admin", quality, w, h);
                        }
                    }

                    Admin admin = _uow.Admin.GetById(entity.Id);
                    admin.LastLoginDate = DateTime.Now;
                    admin.LastLoginIp = HttpContext.Connection.RemoteIpAddress.ToString();
                    admin.Password = Cipher.Encrypt(entity.Password, entity.UserName);
                    admin.PinCode = entity.PinCode;

                    admin.Name = entity.Name;
                    admin.Surname = entity.Surname;
                    admin.UserName = entity.UserName;
                    admin.Adress = entity.Adress;
                    admin.Email = email;
                    admin.Birthday = entity.Birthday;
                    admin.Image = entity.Image;
                    admin.Phone = entity.Phone;

                    _uow.Admin.Update(admin);
                    return Redirect("/admin/home/index");
                }
                else
                {
                    if (controlUserName == false && controlUserEmail == false)
                    {
                        ViewBag.ErrorMessage = "Bu kullanıcı adı ve email adresi sistemimizde kayıtlıdır.";
                    }
                    else if (controlUserName == false)
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
            else
            {
                return View(entity);
            }
        }

        [Authorize(Policy = "Staff")]
        [HttpGet]
        public IActionResult ProfileStaff()
        {
            var id = _uow.Admin.GetIdByAdmin;

            Staff staff = _uow.Staff.GetById(id);
            var decryptedPassword = Cipher.Decrypt(staff.Password, staff.UserName);
            staff.Password = decryptedPassword;

            return View(staff);
        }

        [Authorize(Policy = "Staff")]
        [HttpPost]
        public IActionResult ProfileStaff(Staff entity, IFormFile Image)
        {
            string filePath = "/img/staff/";
            int quality = 80;
            int w = 500;
            int h = 500;
            if (ModelState.IsValid)
            {
                string email = entity.Email.ToLower();
                var controlUserName = _uow.Admin.ControlUserName(entity.UserName, "staff", entity.Id);
                var controlUserEmail = _uow.Admin.ControlUserEmail(email, "staff", entity.Id);

                if (controlUserEmail == true && controlUserName == true)
                {
                    if (Image != null)
                    {
                        if (_uow.Media.ChangeFileName(Image.FileName, Image) != entity.Image)
                        {
                            _uow.Media.DeleteImage(entity.Image, "admin", filePath);
                            entity.Image = _uow.Media.ChangeFileName(entity.UserName, Image);
                            _uow.Media.InsertWithIFormFile(filePath, entity.UserName, "admin", Image, "staff", quality, w, h);
                        }
                    }

                    Staff staff = _uow.Staff.GetById(entity.Id);

                    staff.Adress = entity.Adress;
                    staff.DepartmentId = entity.DepartmentId;
                    staff.Email = email;
                    staff.Image = entity.Image;
                    staff.Name = entity.Name;
                    staff.Surname = entity.Surname;
                    staff.Phone = entity.Phone;
                    staff.UserName = entity.UserName;

                    staff.LastLoginDate = DateTime.Now;
                    staff.LastLoginIp = HttpContext.Connection.RemoteIpAddress.ToString();
                    staff.Password = Cipher.Encrypt(entity.Password, entity.UserName);
                    staff.PinCode = entity.PinCode;

                    _uow.Staff.Update(staff);
                    return Redirect("/admin/home/index");
                }
                else
                {
                    if (controlUserName == false && controlUserEmail == false)
                    {
                        ViewBag.ErrorMessage = "Bu kullanıcı adı ve email adresi sistemimizde kayıtlıdır.";
                    }
                    else if (controlUserName == false)
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
            else
            {
                return View(entity);
            }

        }

        [Authorize(Policy = "Current")]
        [HttpGet]
        public IActionResult ProfileCurrent()
        {
            var id = _uow.Admin.GetIdByAdmin;

            Current current = _uow.Current.GetById(id);
            string decryptedPassword = Cipher.Decrypt(current.Password, current.UserName);
            current.Password = decryptedPassword;

            return View(current);
        }

        [Authorize(Policy = "Current")]
        [HttpPost]
        public IActionResult ProfileCurrent(Current entity, IFormFile Image)
        {
            string filePath = "/img/current/";
            int quality = 80;
            int w = 500;
            int h = 500;
            int id = _uow.Admin.GetIdByAdmin;
            if (ModelState.IsValid)
            {
                string email = entity.Mail.ToLower();
                var controlUserName = _uow.Admin.ControlUserName(entity.UserName, "current", entity.Id);
                var controlUserEmail = _uow.Admin.ControlUserEmail(email, "current", entity.Id);

                if (controlUserEmail == true && controlUserName == true)
                {
                    if (Image != null)
                    {
                        if (_uow.Media.ChangeFileName(Image.FileName, Image) != entity.Image)
                        {
                            _uow.Media.DeleteImage(entity.Image, "admin", filePath);
                            entity.Image = _uow.Media.ChangeFileName(entity.UserName, Image);
                            _uow.Media.InsertWithIFormFile(filePath, entity.UserName, "admin", Image, "current", quality, w, h);
                        }
                    }
                    Current current = _uow.Current.GetById(entity.Id);
                    current.City = entity.City;
                    current.Image = entity.Image;
                    current.Mail = email;
                    current.Name = entity.Name;
                    current.Surname = entity.Surname;
                    current.UserName = entity.UserName;

                    current.LastLoginDate = DateTime.Now;
                    current.LastLoginIp = HttpContext.Connection.RemoteIpAddress.ToString();
                    current.Password = Cipher.Encrypt(entity.Password, entity.UserName);
                    current.PinCode = entity.PinCode;

                    _uow.Current.Update(current);
                    return Redirect("/admin/home/index");
                }
                else
                {
                    if (controlUserName == false && controlUserEmail == false)
                    {
                        ViewBag.ErrorMessage = "Bu kullanıcı adı ve email adresi sistemimizde kayıtlıdır.";
                    }
                    else if (controlUserName == false)
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
            else
            {
                return View(entity);
            }
        }


        [HttpPost]
        public JsonResult Get()
        {
            var setting = _uow.Setting.GetByLang(_uow.Cookie.GetCookie("Language"));
            return Json(setting);
        }
    }
}
