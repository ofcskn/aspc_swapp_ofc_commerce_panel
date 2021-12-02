using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Abstract
{
    public interface IProductService : IGenericService<Product>
    {
        Product GetProductById(int id);
        int GetIdByBarcode(string bardoce);
        IQueryable<Product> GetAllPC();
        List<Product> GetAllPCByFilter(string q);
        bool ControlBarcode(string barcode);
        Product GetProductByBarcode(string bardoce);
        IQueryable<ProductImage> GetAllProductImage(int pid);
    }
}
