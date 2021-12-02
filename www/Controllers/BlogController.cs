using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using www.Models;
using Service.Utilities;
using Service.Abstract;
using Microsoft.AspNetCore.Http;

namespace www.Controllers
{
    public class BlogController : Controller
    {
        private readonly IUnitOfWork _uow;
        public BlogController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        [Route("/blog/{permalink}")]
        public IActionResult Detail(string permalink)
        {
            var blog = _uow.Blog.GetByPermalink(permalink);
            if (blog.Enabled == true)
            {
                return View(blog);
            }
            return Redirect("/home/blog");
        }
    }
}
