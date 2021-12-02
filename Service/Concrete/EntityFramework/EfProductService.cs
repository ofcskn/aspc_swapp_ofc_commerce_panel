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
    public class EfProductService : EfGenericService<Product>, IProductService
    {
        public EfProductService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public IQueryable<ProductImage> GetAllProductImage(int pid)
        {
            return _db.ProductImage.Where(p=>p.ProductId == pid);
        }
        public bool ControlBarcode(string barcode)
        {
            if (_db.Product.FirstOrDefault(p => p.Barcode == barcode) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public IQueryable<Product> GetAllPC()
        {
            return _db.Product.Include(p=>p.Category).Include(p => p.ProductImage).Include(p => p.ProductDescription);
        }
        public List<Product> GetAllPCByFilter(string q)
        {
            List<Product> filtered = GetAllPC().ToList();
            if (!string.IsNullOrEmpty(q) && filtered.Count() > 0)
            {
                filtered = filtered.Where(p => p.Name.Contains(q, StringComparison.InvariantCultureIgnoreCase)
                || Convert.ToString(p.SalePrice).Contains(q)
                || p.Brand.Contains(q, StringComparison.InvariantCultureIgnoreCase)
                || Convert.ToString(p.Stock).Contains(q)
                || Convert.ToString(p.Category.Name).Contains(q)
                || p.ProductDescription.First().Description.Contains(q, StringComparison.InvariantCultureIgnoreCase)
                || p.Barcode.Contains(q)).ToList();
            }
            return filtered;
        }
        public int GetIdByBarcode(string bardoce)
        {
            return _db.Product.FirstOrDefault(p => p.Barcode == bardoce).Id;
        }
        public Product GetProductByBarcode(string bardoce)
        {
            return _db.Product.FirstOrDefault(p => p.Barcode == bardoce);
        }
        public Product GetProductById(int id)
        {
            return _db.Product.Include(p => p.Category).Include(p => p.ProductColor).Include(p => p.ProductDescription)
                .Include(p => p.ProductDescription).Include(p => p.ProductImage).Include(p => p.ProductRating).Include(p=>p.ProductSize)
                .Include(p => p.ProductComment)
                .FirstOrDefault(p=>p.Id == id);
        }
    }
}
