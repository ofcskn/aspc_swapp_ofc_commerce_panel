using AutoMapper;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace www.ViewComponents
{
    public class Project : ViewComponent
    {
        private readonly IUnitOfWork _uow;
        public Project(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public IViewComponentResult Invoke()
        {
            var projects = _uow.Project.GetAll().Where(p => p.Enabled == true).OrderBy(p=>p.Priority);
            return View(projects);
        }
    }
}
