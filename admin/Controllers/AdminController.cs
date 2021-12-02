using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.Abstract;
using Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using admin.Models;
using Service.Utilities;
using Microsoft.AspNetCore.Http;

namespace admin.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _uow;
        public AdminController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<IActionResult> List(string filter, int pageNumber = 1)
        {
            if (filter != null)
            {
                return View(await PaginatedList<Admin>.CreateAsync(_uow.Admin.GetAll().Where(p => p.Role == filter), pageNumber, 10));
            }
            else
            {
                return View(await PaginatedList<Admin>.CreateAsync(_uow.Admin.GetAll(), pageNumber, 10));
            }
        }
        [HttpGet]
        public IActionResult Manage(int? id)
        {
            if (id != null)
            {
                var admin = _uow.Admin.GetById(Convert.ToInt32(id));
                var decryptedPassword = Cipher.Decrypt(admin.Password, admin.UserName);
                admin.Password = decryptedPassword;

                return View(admin);
            }
            else
            {
                return View(new Admin());
            }
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult Manage(Admin entity, IFormFile Image)
        {
            int quality = 60;
            int w = 500;
            int h = 500;
            //For small images
            int ws = 50;
            int hs = 50;
            string filePath = "/img/admin/";
            string fileName = entity.Name + " " + entity.Surname;
            if (ModelState.IsValid)
            {
                string email = entity.Email.ToLower();
                if (_uow.Admin.ControlUserEmail(email, "admin", entity.Id) == true && _uow.Admin.ControlUserName(entity.UserName, "admin", entity.Id) == true)
                {
                    if (entity.Id != 0) //Email Adress Control
                    {
                        if (Image != null)
                        {
                            if (_uow.Media.ChangeFileName(Image.FileName, Image) != entity.Image)
                            {
                                _uow.Media.DeleteImage(entity.Image, "admin", filePath);
                                _uow.Media.DeleteImage(entity.Image + "-s", "admin", filePath);
                                entity.Image = _uow.Media.ChangeFileName(fileName, Image);
                                _uow.Media.InsertWithIFormFile(filePath, fileName, "admin", Image, "admin", quality, w, h);
                                _uow.Media.InsertWithIFormFile(filePath, fileName + "-s", "admin", Image, "admin", quality, ws, hs);
                            }
                        }
                        Admin admin = _uow.Admin.GetById(entity.Id);
                        admin.LastLoginDate = DateTime.Now;
                        admin.LastLoginIp = HttpContext.Connection.RemoteIpAddress.ToString();
                        admin.Password = Cipher.Encrypt(entity.Password, entity.UserName);

                        admin.Name = entity.Name;
                        admin.PinCode = entity.PinCode;
                        admin.Surname = entity.Surname;
                        admin.UserName = entity.UserName;
                        admin.Adress = entity.Adress;
                        admin.Email = email;
                        admin.Birthday = entity.Birthday;
                        admin.Image = entity.Image;
                        admin.Phone = entity.Phone;

                        _uow.Admin.Update(admin);
                        return RedirectToAction("List");
                    }
                    else
                    {
                        if (_uow.Admin.ControlUserEmail(email, "admin", entity.Id) == true && _uow.Admin.ControlUserName(entity.UserName, "admin", entity.Id) == true)
                        {
                            //Upload Image
                            entity.Image = _uow.Media.ChangeFileName(fileName, Image);
                            _uow.Media.InsertWithIFormFile(filePath, fileName, "admin", Image, "admin", quality, w, h);
                            _uow.Media.InsertWithIFormFile(filePath, fileName + "-s", "admin", Image, "admin", quality, ws, hs);

                            entity.UserName = entity.UserName.ToLower();
                            entity.Email = email;
                            entity.Password = Cipher.Encrypt(entity.Password, entity.UserName);
                            entity.LastLoginDate = DateTime.Now;
                            //entity.LastLoginIp = HttpContext.Connection.RemoteIpAddress.ToString();
                            entity.RegisterDate = DateTime.Now;
                            _uow.Admin.Add(entity);

                            //Admin Notification To Admins
                            var nottype = _uow.NotificationType.GetNTByType("new-admin");
                            string notificationTitle = entity.Name + " " + entity.Surname + " " + nottype.Message;
                            _uow.Notification.AddListForAdmin(nottype.Id, notificationTitle, _uow.Admin.GetAllByEnabled(true));

                            return RedirectToAction("List");
                        }
                        else
                        {
                            if (_uow.Admin.ControlUserName(entity.UserName, "admin", entity.Id) == false)
                            {
                                ViewBag.ErrorMessage = "Bu kullanıcı adında kayıtlı bir kullanıcı vardır.";
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "Bu mail adresinde kayıtlı bir kullanıcı vardır.";
                            }
                            return View();
                        }
                    }
                }
                else
                {
                    if (_uow.Admin.ControlUserName(entity.UserName, "admin", entity.Id) == false && _uow.Admin.ControlUserEmail(email, "admin", entity.Id) == false)
                    {
                        ViewBag.ErrorMessage = "Bu kullanıcı adı ve email adresi sistemimizde kayıtlıdır.";
                    }
                    else if (_uow.Admin.ControlUserName(entity.UserName, "admin", entity.Id) == false)
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
        [Authorize(Policy = "Admin")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var Admin = _uow.Admin.GetById(Convert.ToInt32(id));
            return View(Admin);
        }
        [Authorize(Policy = "Admin")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var admin = _uow.Admin.GetById(Convert.ToInt32(id));
            if (admin != null)
            {
                if (admin.Image != null)
                {
                    //_uow.Media.DeleteImage(admin.Image, "admin", "/img/admin/");
                    _uow.Admin.Delete(admin);
                    return RedirectToAction("List");
                }
            }
            return View(admin);
        }
    }
}
