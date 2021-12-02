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

namespace admin.Controllers
{
    [Authorize(Policy = "Admin")]
    public class FileManagerController : Controller
    {
        private readonly IUnitOfWork _uow;
        public FileManagerController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Index2(int? id)
        {
            //ViewBag.fileRoot = "\"C:/Users/LENOVO/source/repos/Swapp/admin/wwwroot/\"";
            ViewBag.user_id = "123";
            ViewBag.fileRoot = "\"/admin-lte/\"";
            //ViewBag.fileRoot = "\"c:/AMD/\"";
            //ViewBag.fileRoot = "\"//Breath/mssqlserver/FileTableDB/FileTableTb_Dir/userfiles/\"";            
            ViewBag.serverRoot = "true";
            ViewBag.serverMode = "false";
            ViewBag.useFileTable = "false";


            return View();
        }
    }
}
