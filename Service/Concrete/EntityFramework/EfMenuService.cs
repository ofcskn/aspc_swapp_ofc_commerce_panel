
using Entity.Context;
using Entity.Models;
using Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Concrete.EntityFramework
{
    public class EfMenuService : EfGenericService<Menu>, IMenuService
    {
        public EfMenuService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }

        public IQueryable<Menu> GetAllByPriority(bool status)
        {
            return _db.Menu.Where(p=>p.Enabled == status).OrderBy(p=>p.Priority);
        }
    }
}
