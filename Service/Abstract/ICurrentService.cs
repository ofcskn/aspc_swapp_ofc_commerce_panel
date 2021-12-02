using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface ICurrentService : IGenericService<Current>
    {
        Current GetByNo(string memberNo);
        bool ControlMembership(string memberNo);
        IQueryable<Current> GetAllByEnabled(bool status);
        Current GetCurrent(string username, string password);
        Current GetByIdCurrent(int id);
    }
}
