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

namespace admin.Controllers
{
    [Authorize]
    public class ExportedController : Controller
    {
        private readonly IUnitOfWork _uow;
        public ExportedController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public IActionResult List(string filter)
        {
            if (filter == "staff")
            {
            return View("StaffList",_uow.Staff.GetAllByDate());
            }
            else if (filter == "current")
            {
                return View("currentlist",_uow.Current.GetAll());
            }
            else if (filter =="product")
            {
                return View("ProductList",_uow.Product.GetAllPC());
            }
            else if (filter =="saleprocess")
            {
                return View("SaleProcessList", _uow.SaleProcess.GetAllByDate());
            }
            else
            {
                return Redirect("/admin/home/index");
            }
        }
    }
}
