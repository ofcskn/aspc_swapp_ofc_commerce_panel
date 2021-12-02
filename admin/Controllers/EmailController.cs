using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using admin.Models;
using admin.ViewModel;
using Entity.Models;
using Entity.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service.Abstract;
using Service.Utilities;

namespace admin.Controllers
{
    [Authorize]
    public class EmailController : Controller
    {
        private readonly IUnitOfWork _uow;
        public EmailController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IActionResult> Inbox(string filter, string q, int pageNumber = 1)
        {
            EmailInboxViewModel evm = new EmailInboxViewModel();
            var list = _uow.Email.GetAllReceivedMail(Convert.ToInt32(_uow.Admin.GetIdByAdmin), _uow.Admin.GetRoleByAdmin);
            if (filter == "receive")
            {
                list = _uow.Email.GetAllReceivedMail(Convert.ToInt32(_uow.Admin.GetIdByAdmin), _uow.Admin.GetRoleByAdmin);
            }
            if (filter == "send")
            {
                list = _uow.Email.GetAllSendedMail(Convert.ToInt32(_uow.Admin.GetIdByAdmin), _uow.Admin.GetRoleByAdmin);
            }
            if (filter == "draft")
            {
                list = _uow.Email.GetAllDraftMail(Convert.ToInt32(_uow.Admin.GetIdByAdmin), _uow.Admin.GetRoleByAdmin);
            }
            if (filter == "trash")
            {
                list = _uow.Email.GetAllRubbishMail(Convert.ToInt32(_uow.Admin.GetIdByAdmin), _uow.Admin.GetRoleByAdmin);
            }
            if (filter == "favourite")
            {
                list = _uow.Email.GetAllFavouriteMail(Convert.ToInt32(_uow.Admin.GetIdByAdmin), _uow.Admin.GetRoleByAdmin);
            }
            if (q != null)
            {
                list = _uow.Email.GetEmailResultBySearch(q, list);
            }
            evm.Emails = await PaginatedList<Email>.CreateAsync(list, pageNumber, 10);
            evm.Filter = filter == null ? "receive" : filter;
            return View(evm);
        }

        [HttpGet]
        public IActionResult Compose(int? id)
        {
            if (id != null)
            {
                return View(_uow.Email.GetEmail(Convert.ToInt32(id)));
            }
            else
            {
                return View(new Email());
            }
        }
        [HttpPost]
        public IActionResult Compose(Email entity, string ReceiverEmail, string filter, IEnumerable<IFormFile> attachments)
        {
            if (filter == "draft")
            {
                if (entity.Subject != null || entity.Description != null || ReceiverEmail != null)
                {
                    if (ReceiverEmail != null)
                    {
                        entity.ReceiverId = Convert.ToInt32(_uow.Email.GetReceiverInfoByEmail(ReceiverEmail, "id"));
                        entity.ReceiverRole = _uow.Email.GetReceiverInfoByEmail(ReceiverEmail, "role");
                        if (entity.ReceiverId == null || entity.ReceiverRole == null || (entity.ReceiverId == _uow.Admin.GetIdByAdmin && entity.ReceiverRole == _uow.Admin.GetRoleByAdmin))
                        {
                            if (entity.ReceiverId == _uow.Admin.GetIdByAdmin && entity.ReceiverRole == _uow.Admin.GetRoleByAdmin)
                            {
                                return Json("same-email");
                            }
                            else
                            {
                                return Json("wrong-email");
                            }
                        }
                    }
                    if (entity.Id != 0)
                    {
                        var email = _uow.Email.GetById(entity.Id);
                        entity.SenderName = _uow.Admin.GetUserNameSurname(entity.SenderRole, entity.SenderId, "name");
                        email.ReceiverId = entity.ReceiverId;
                        email.ReceiverRole = entity.ReceiverRole;
                        email.Subject = entity.Subject;
                        email.Description = entity.Description;
                        _uow.Email.Update(email);
                        return Json(entity);
                    }
                    else
                    {
                        entity.DraftDate = DateTime.Now;
                        entity.DraftEnabled = true;
                        entity.SenderId = _uow.Admin.GetIdByAdmin;
                        entity.SendDate = DateTime.Now;
                        entity.SenderRole = _uow.Admin.GetRoleByAdmin;
                        entity.SenderName = _uow.Admin.GetUserNameSurname(entity.SenderRole, entity.SenderId, "name");
                        _uow.Email.Add(entity);

                        return Json(entity);
                    }
                }
                return Json("error");
            }
            else if (ModelState.IsValid)
            {
                entity.ReceiverId = Convert.ToInt32(_uow.Email.GetReceiverInfoByEmail(ReceiverEmail, "id"));
                entity.ReceiverRole = _uow.Email.GetReceiverInfoByEmail(ReceiverEmail, "role");
                if (entity.ReceiverId != 0 || entity.ReceiverRole != null)
                {
                    if (entity.ReceiverId == _uow.Admin.GetIdByAdmin && entity.ReceiverRole == _uow.Admin.GetRoleByAdmin)
                    {
                        ViewBag.ErrorMessage = "Kendinize e-posta gönderemezsiniz.";
                        return View(entity);
                    }
                    else
                    {
                        if (entity.Id != 0)
                        {
                            var email = _uow.Email.GetById(entity.Id);
                            email.Subject = entity.Subject;
                            email.Description = entity.Description;
                            email.ReceiverId = entity.ReceiverId;
                            email.ReceiverRole = entity.ReceiverRole;
                            email.SendDate = DateTime.Now;
                            entity.SenderName = _uow.Admin.GetUserNameSurname(entity.SenderRole, entity.SenderId, "name");
                            email.DraftEnabled = false;
                            _uow.Email.Update(email);

                            if (attachments != null)
                            {
                                if (attachments.Count() < 5)
                                {

                                    if (attachments.Sum(p => p.Length) < 10485760)
                                    {
                                        _uow.Media.AddEmailAttachments(entity, attachments);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMessage = "Dosyalarınızın toplam boyutu 10 MB altında olmalıdır.";
                                        return View(entity);
                                    }
                                }
                                else
                                {
                                    ViewBag.ErrorMessage = "En fazla 4 dosya yükleyebilirsiniz.";
                                    return View(entity);
                                }
                            }

                            _uow.EmailStatus.AddEmailStatus(email.Id, Convert.ToInt32(email.ReceiverId), email.ReceiverRole);

                            var nottype = _uow.NotificationType.GetNTByType("compose-email");
                            try
                            {
                                //Add Notification
                                Notification notification = new Notification
                                {
                                    Enabled = false,
                                    NotTypeId = nottype.Id,
                                    SendDate = DateTime.Now,
                                    Title = _uow.Admin.GetUserNameSurname(_uow.Admin.GetRoleByAdmin, _uow.Admin.GetIdByAdmin, "name") + " " + nottype.Message,
                                    UserId = Convert.ToInt32(entity.ReceiverId),
                                    UserRole = entity.ReceiverRole
                                };
                                _uow.Notification.Add(notification);
                            }
                            catch (Exception)
                            {
                            }
                            return RedirectToAction("Inbox");
                        }
                    }
                }
                ViewBag.ErrorMessage = "Girdiğiniz e-posta adresi sistemimizde kayıtlı değildir. Lütfen tekrar deneyin.";
                return View(entity);
            }
            else
            {
                ViewBag.ErrorMessage = "Lütfen gerekli alanları doldurun.";
                return View(entity);
            }
        }
        public JsonResult ChangeStatus(int emailId, bool status, string filter)
        {
            _uow.EmailStatus.ChangeStatus(emailId, _uow.Admin.GetRoleByAdmin, _uow.Admin.GetIdByAdmin, filter, status);
            return Json(status);
        }
        [HttpGet]
        public IActionResult Read(int id)
        {
            Email email = _uow.Email.GetEmail(id);
            if (email != null)
            {
                var emailStatus = _uow.EmailStatus.GetEmailStatus(id, _uow.Admin.GetRoleByAdmin, _uow.Admin.GetIdByAdmin);
                emailStatus.ReadDate = DateTime.Now;
                _uow.EmailStatus.ChangeStatus(id, _uow.Admin.GetRoleByAdmin, _uow.Admin.GetIdByAdmin, "read", true);
            }
            return View(email);
        }
        public ActionResult DownloadAttachment(string filename)
        {
            if (filename != null)
            {
                var filePath = "/uploads/email/";
                filePath = Helpers.ChangeFilePath(filePath);
                var fileUploadDirectory = _uow.Media.FileUploadDirectory("admin") + filePath + filename;

                byte[] fileBytes = System.IO.File.ReadAllBytes(fileUploadDirectory);

                return File(fileBytes, "application/force-download", filename);
            }
            return Json("no");
        }
        [HttpPost]
        public ActionResult Delete(int emailId)
        {
            _uow.EmailStatus.ChangeStatus(emailId, _uow.Admin.GetRoleByAdmin, _uow.Admin.GetIdByAdmin, "permanent-trash", true);
            return Json("sadasd");
        }

    }
}
