
using Entity.Context;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Concrete.EntityFramework
{
    public class EfSaleProcessService : EfGenericService<SaleProcess>, ISaleProcessService
    {
        public EfSaleProcessService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public IQueryable<SaleProcess> GetAllByStaff(int sellerId)
        {
            return _db.SaleProcess.Where(p => p.StaffId == sellerId);
        }
        public IQueryable<SaleProcess> GetAllByDate()
        {
            return _db.SaleProcess.OrderByDescending(p => p.Date);
        }
        public void UpdateProductStock(SaleProcess entity, int productId, int staffId)
        {
            entity.Status = true;
            entity.Date = DateTime.Now;
            entity.StaffId = staffId;
            Product product = _db.Product.FirstOrDefault(p=>p.Id == productId);
            int stock = Convert.ToInt32(product.Stock);
            int amount = Convert.ToInt32(entity.Amount);
            product.Stock = (stock - amount).ToString();
            _db.Product.Update(product);
            Add(entity);
        }
    }
}
