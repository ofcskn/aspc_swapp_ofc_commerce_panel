using admin.ViewModel;
using admin.ViewModels;
using AutoMapper;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admin.ViewComponents
{
    public class TopNavbar: ViewComponent
    {
        private readonly IUnitOfWork _uow;
        public TopNavbar(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public IViewComponentResult Invoke()
        {
            var userId = _uow.Admin.GetIdByAdmin;
            var userRole = _uow.Admin.GetRoleByAdmin;

            var newEmailList = new List<EmailMessageViewModel>();
            foreach (var item in _uow.Email.GetAllReceivedByRead(userId, userRole, false))
            {
                EmailMessageViewModel emailMessageViewModel = new EmailMessageViewModel();
                if (userRole == "admin" || userRole == "user")
                {
                    emailMessageViewModel.SenderName = _uow.Admin.GetById(userId).Name + " " + _uow.Admin.GetById(userId).Surname;
                }
                if (userRole == "current")
                {
                    emailMessageViewModel.SenderName = _uow.Current.GetById(userId).Name + " " + _uow.Current.GetById(userId).Surname;
                }
                if (userRole == "staff")
                {
                    emailMessageViewModel.SenderName = _uow.Staff.GetById(userId).Name + " " + _uow.Staff.GetById(userId).Surname;
                }
                emailMessageViewModel.Id = item.Id;
                emailMessageViewModel.Subject = item.Subject;
                emailMessageViewModel.SendDate = Convert.ToDateTime(item.SendDate);
                emailMessageViewModel.Description = item.Description;
                newEmailList.Add(emailMessageViewModel);
            }

            NavbarViewModel nvm = new NavbarViewModel();
            nvm.Emails = newEmailList.Take(3).ToList();

            nvm.NotReadEmailsCount = newEmailList.Count();
            nvm.NotReadCargoCompanyCount = _uow.Notification.GetAllByUser(_uow.Admin.GetIdByAdmin, _uow.Admin.GetRoleByAdmin).Include(p=>p.NotType).Where(p => p.NotType.GeneralType == "Cargo Company" && p.ReadEnabled == false).Count();
            nvm.NotReadMessageCount = _uow.Notification.GetAllByUser(_uow.Admin.GetIdByAdmin, _uow.Admin.GetRoleByAdmin).Include(p=>p.NotType).Where(p => p.NotType.GeneralType == "Message" && p.ReadEnabled == false).Count();
            nvm.NotificationCount = _uow.Notification.GetAllByUser(_uow.Admin.GetIdByAdmin,_uow.Admin.GetRoleByAdmin).Where(p=>p.ReadEnabled == false).Count();
            return View(nvm);
        }
    }
}
