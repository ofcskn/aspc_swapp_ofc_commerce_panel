using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface INotificationService : IGenericService<Notification>
    {
        IQueryable<Notification> GetAllByUser(int userId, string userRole);
        void AddListForStaff(int nottypeid, string nottitle, IQueryable<Staff> list);
        void AddListForAdmin(int nottypeid, string nottitle, IQueryable<Admin> list);
        void AddListForCurrent(int nottypeid, string nottitle, IQueryable<Current> list);
    }
}
