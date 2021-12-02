using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using admin.Models;
using admin.ViewModel;
using Entity.Models;
using Entity.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service.Abstract;
using Service.Utilities;

namespace admin.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IUnitOfWork _uow;
        public MessageController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IActionResult Chat()
        {
            return View();
        }
    }
}
