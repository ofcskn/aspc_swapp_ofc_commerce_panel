using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IMenuService : IGenericService<Menu>
    {
        IQueryable<Menu> GetAllByPriority(bool status);
    }
}
