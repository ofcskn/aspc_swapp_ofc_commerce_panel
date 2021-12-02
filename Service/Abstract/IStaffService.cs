using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IStaffService : IGenericService<Staff>
    {
        IQueryable<Staff> GetAllByDepartment(int depId);
        IQueryable<Staff> GetAllByDate();
        IQueryable<Staff> GetAllByEnabled();
        Staff GetStaff(string username, string password);
    }
}
