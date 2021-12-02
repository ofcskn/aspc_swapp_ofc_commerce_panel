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
    public class EfContactService : EfGenericService<Contact>, IContactService
    {
        public EfContactService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }

        public IQueryable<Contact> GetAllByDate()
        {
            return _db.Contact.OrderByDescending(p=>p.Date);
        }
    }
}
