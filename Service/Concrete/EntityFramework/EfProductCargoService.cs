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
    public class EfProductCargoService : EfGenericService<ProductCargo>, IProductCargoService
    {
        public EfProductCargoService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }

        public IQueryable<ProductCargo> GetAllPC()
        {
            return _db.ProductCargo.Include(p => p.CargoCompany).Include(p=>p.Current).OrderByDescending(p=>p.StartDate);
        }
        //Filter For Search
        public IQueryable<ProductCargo> SearchByCargoCode(string code)
        {
            return _db.ProductCargo.Include(p => p.CargoCompany).Include(p=>p.Current).OrderByDescending(p=>p.StartDate).Where(p=>p.CargoNo.Contains(code));
        }
        //Completed and Non-Completed Cargo Process
        public IQueryable<ProductCargo> GetAllPCByEnabled(bool filter)
        {
            return _db.ProductCargo.Include(p => p.CargoCompany).Include(p => p.Current).Where(p=>p.Enabled == filter);
        }
        public IQueryable<ProductCargo> GetAllPCByc(int companyId)
        {
            return _db.ProductCargo.Include(p => p.CargoCompany).Include(p => p.Current).Where(p => p.CargoCompanyId == companyId);
        }
    }
}
