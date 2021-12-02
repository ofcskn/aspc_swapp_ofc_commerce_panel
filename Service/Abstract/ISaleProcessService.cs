using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface ISaleProcessService : IGenericService<SaleProcess>
    {
        IQueryable<SaleProcess> GetAllByStaff(int sellerId);
        IQueryable<SaleProcess> GetAllByDate();
        void UpdateProductStock(SaleProcess entity, int productId, int staffId);
    }
}
