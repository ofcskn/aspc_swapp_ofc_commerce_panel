
using Entity.Context;
using Entity.Models;
using Service.Abstract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Concrete.EntityFramework
{
    public class EfNewsletterService : EfGenericService<Newsletter>, INewsletterService
    {
        public EfNewsletterService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
    }
}
