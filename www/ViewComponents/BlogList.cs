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
    public class BlogList : ViewComponent
    {
        private readonly IUnitOfWork _uow;
        public BlogList(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public IViewComponentResult Invoke()
        {
            var bloglist = _uow.Blog.GetAll().Where(p=>p.Enabled == true).OrderByDescending(p=>p.Date);
            return View(bloglist);
        }
    }
}
