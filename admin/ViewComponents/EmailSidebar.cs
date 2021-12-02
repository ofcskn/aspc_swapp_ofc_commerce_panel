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
    public class EmailSidebar : ViewComponent
    {
        private readonly IUnitOfWork _uow;
        public EmailSidebar(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public IViewComponentResult Invoke()
        {
            EmailSideBarViewModel vm = new EmailSideBarViewModel {
                TotalReceived = _uow.Email.GetAllReceivedMail(Convert.ToInt32(_uow.Admin.GetIdByAdmin), _uow.Admin.GetRoleByAdmin).Count(),
                TotalSended = _uow.Email.GetAllSendedMail(Convert.ToInt32(_uow.Admin.GetIdByAdmin), _uow.Admin.GetRoleByAdmin).Count(),
                TotalDrafted = _uow.Email.GetAllDraftMail(Convert.ToInt32(_uow.Admin.GetIdByAdmin), _uow.Admin.GetRoleByAdmin).Count(),
                TotalFavourite = _uow.Email.GetAllFavouriteMail(Convert.ToInt32(_uow.Admin.GetIdByAdmin), _uow.Admin.GetRoleByAdmin).Count(),
                TotalTrash = _uow.Email.GetAllRubbishMail(Convert.ToInt32(_uow.Admin.GetIdByAdmin), _uow.Admin.GetRoleByAdmin).Count()
            };
            return View(vm);
        }
    }
}
