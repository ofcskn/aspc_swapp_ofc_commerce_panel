using Service.Abstract;
using Entity.Context;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Service.Concrete.EntityFramework
{
    public class EfCargoCompanyService : EfGenericService<CargoCompany>, ICargoCompanyService
    {
        public EfCargoCompanyService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public IQueryable<CargoCompany> GetAllPC()
        {
            return _db.CargoCompany.Include(p=>p.ProductCargo);
        }
    }
}
