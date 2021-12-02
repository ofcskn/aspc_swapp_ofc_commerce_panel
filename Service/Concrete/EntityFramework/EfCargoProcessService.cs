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
    public class EfCargoProcessService : EfGenericService<CargoProcess>, ICargoProcessService
    {
        public EfCargoProcessService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }

        public IQueryable<CargoProcess> GetAllByEnabledDate(bool status)
        {
            return _db.CargoProcess.Where(p => p.Enabled == status).OrderByDescending(p => p.Date);
        }
        public IQueryable<CargoProcess> GetAllByEnabled(bool status)
        {
            return _db.CargoProcess.Where(p => p.Enabled == status);
        }
        public IQueryable<CargoProcess> GetAllByCargo(int cargoid)
        {
            return _db.CargoProcess.Where(p => p.ProductCargoId == cargoid);
        }
    }
}
