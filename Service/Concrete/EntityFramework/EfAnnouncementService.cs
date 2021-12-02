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
    public class EfAnnouncementService : EfGenericService<Announcement>, IAnnouncementService
    {
        public EfAnnouncementService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }

        public IQueryable<Announcement> GetAllByEnabledDate(bool status)
        {
            return _db.Announcement.Where(p => p.Enabled == status).OrderByDescending(p=>p.Date);
        }
        public IQueryable<Announcement> GetAllByEnabled(bool status)
        {
            return _db.Announcement.Where(p=>p.Enabled == status);
        }
    }
}
