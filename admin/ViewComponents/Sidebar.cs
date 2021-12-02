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
    public class Sidebar : ViewComponent
    {
        private readonly IUnitOfWork _uow;
        public Sidebar(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public IViewComponentResult Invoke()
        {
            MenuViewModel vm = new MenuViewModel
            {
                Menus = _uow.Menu.GetAllByPriority(true).Where(p=>p.Role.Contains(_uow.Admin.GetRoleByAdmin)),
                userRole = _uow.Admin.GetRoleByAdmin,
                userImage = _uow.Admin.GetImageByAdmin,
                userName = _uow.Admin.GetNameByAdmin
            };
            return View(vm);
        }
    }
}
