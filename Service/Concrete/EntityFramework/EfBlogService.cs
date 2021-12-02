using Entity.Context;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Service.Abstract;
using Service.Utilities;

namespace Service.Concrete.EntityFramework
{
    public class EfBlogService : EfGenericService<Blog>, IBlogService
    {
        public EfBlogService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public Blog GetByPermalink(string permalink)
        {
            return _db.Blog.FirstOrDefault(p=>p.Permalink == permalink);
        }
        public string GenerateAndInsertPermalink(string title)
        {
            string permalink = Helpers.ToSlug(title);

            if (_db.Blog.Any(e => e.Permalink == permalink))
            {
                string lastChar = permalink.Substring(permalink.Length - 1);

                if (lastChar.All(char.IsDigit))
                {
                    int lastIndexOf = permalink.LastIndexOf(lastChar);
                    int count = lastChar.Length;

                    string permalinkWithoutLastChar = permalink.Remove(lastIndexOf, count);

                    permalink = permalinkWithoutLastChar + (Convert.ToInt32(lastChar) + 1).ToString();
                }
                else
                {
                    permalink = permalink + "-1";
                }

                return GenerateAndInsertPermalink(permalink);
            }
            return permalink;

        }

    }
}
