using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface ICargoProcessService : IGenericService<CargoProcess>
    {
        IQueryable<CargoProcess> GetAllByEnabled(bool status);
        IQueryable<CargoProcess> GetAllByEnabledDate(bool status);
        IQueryable<CargoProcess> GetAllByCargo(int cargoid);
    }
}
