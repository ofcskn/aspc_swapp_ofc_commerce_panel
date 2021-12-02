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
using admin.Models;

namespace admin.Controllers
{
    [Authorize]
    public class CurrentController : Controller
    {
        private readonly IUnitOfWork _uow;
        public CurrentController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public JsonResult PublishStatus(bool enabled, int currentId)
        {
            var item = _uow.Current.GetById(currentId);
            if (item != null)
            {
                item.Status = enabled;
                _uow.Current.Update(item);
                if (enabled == true)
                {
                    return Json("start");
                }
                return Json("pause");
            }
            return Json("no");
        }
        [Authorize(Policy = "Staff")]
        public IActionResult ControlMemberCode(string code)
        {
            if (_uow.Current.ControlMembership(code) == true)
            {
                return Json("true");
            }
            else
            {
                return Json("false");
            }
        }
        [Authorize(Policy = "Staff")]
        [HttpGet]
        public async Task<IActionResult> List(int? currentId, int pageNumber = 1)
        {
            if (currentId != null)
            {
                var list = _uow.Current.GetAll();//Your list
                return View(await PaginatedList<Current>.CreateAsync(list, pageNumber, 20));
            }
            else
            {
                var list = _uow.Current.GetAll();//Your list
                return View(await PaginatedList<Current>.CreateAsync(list, pageNumber, 20));
            }
        }
        [Authorize(Policy = "Admin")]
        [HttpGet]
        public IActionResult Manage(int? id)
        {
            if (id != null)
            {
                return View(_uow.Current.GetById(Convert.ToInt32(id)));
            }
            return View(new Current());
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public IActionResult Manage(Current entity)
        {
            if (entity.Id == 0)
            {
                if (ModelState.IsValid)
                {
                    string email = entity.Mail.ToLower();

                    if (_uow.Admin.ControlUserEmail(email, "current", entity.Id) == true && _uow.Admin.ControlUserName(entity.UserName, "current", entity.Id) == true)
                    {
                        Random randomNumber = new Random();
                        String serialRandom = randomNumber.Next(0, 1000000).ToString("D6");
                        var item_list = _uow.Current.GetAll().Where(p => p.No == serialRandom).ToList();
                        if (item_list.Count() != 0)
                        {
                            serialRandom = randomNumber.Next(0, 1000000).ToString("D6");
                        }
                        //entity.Status = false;
                        entity.LastLoginIp = _uow.Admin.GetIp();
                        entity.LastLoginDate = DateTime.Now;
                        entity.No = serialRandom;
                        entity.UserName = entity.UserName.ToLower();
                        entity.Mail = email;
                        entity.RegisterDate = DateTime.Now;
                        entity.LastLoginDate = DateTime.Now;
                        entity.Password = Cipher.Encrypt(entity.Password, entity.UserName);
                        _uow.Current.Add(entity);

                        //Current Notification To Admins
                        var nottype = _uow.NotificationType.GetNTByType("new-current");
                        string notificationTitle = entity.Name + " " + entity.Surname + " " + nottype.Message;
                        try
                        {
                            _uow.Notification.AddListForAdmin(nottype.Id, notificationTitle, _uow.Admin.GetAllByEnabled(true));
                        }
                        catch (Exception)
                        {
                        }

                        return RedirectToAction("List");
                    }
                    else
                    {
                        if (_uow.Admin.ControlUserName(entity.UserName, "current", entity.Id) == false && _uow.Admin.ControlUserEmail(email, "current", entity.Id) == false)
                        {
                            ViewBag.ErrorMessage = "Bu kullanıcı adı ve email adresi sistemimizde kayıtlıdır.";
                        }
                        else if (_uow.Admin.ControlUserName(entity.UserName, "current", entity.Id) == false)
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
            return View(entity);
        }
        [Authorize(Policy = "Admin")]
        public IActionResult Delete(int id)
        {
            var item = _uow.Current.GetById(id);
            _uow.Current.Delete(item);

            //Current Notification To Admins
            var nottype = _uow.NotificationType.GetNTByType("deleted-current");
            string notificationTitle = item.Name + " " + item.Surname + " " + nottype.Message;
            _uow.Notification.AddListForAdmin(nottype.Id, notificationTitle, _uow.Admin.GetAllByEnabled(true));

            return Json("ok");
        }
    }
}
