using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IProductCargoService : IGenericService<ProductCargo>
    {
        IQueryable<ProductCargo> GetAllPC();
        IQueryable<ProductCargo> GetAllPCByEnabled(bool filter);
        IQueryable<ProductCargo> GetAllPCByc(int companyId);
        IQueryable<ProductCargo> SearchByCargoCode(string code);
    }
}
