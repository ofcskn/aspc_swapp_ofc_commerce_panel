
using Entity.Context;
using Entity.Models;
using Service.Abstract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Concrete.EntityFramework
{
    public class EfNotificationService : EfGenericService<Notification>, INotificationService
    {
        public EfNotificationService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public IQueryable<Notification> GetAllByUser(int userId, string userRole)
        {
            return _db.Notification.Where(p => p.UserId == userId && p.UserRole == userRole);
        }
        public void AddListForStaff(int nottypeid, string nottitle, IQueryable<Staff> list)
        {
            try
            {
                foreach (var item in list)
                {
                    //Add Notification
                    Notification notification = new Notification
                    {
                        Enabled = false,
                        NotTypeId = nottypeid,
                        SendDate = DateTime.Now,
                        Title = nottitle,
                        UserId = Convert.ToInt32(item.Id),
                        UserRole = "staff"
                    };
                    _db.Notification.Add(notification);
                }
                _db.SaveChanges();
            }
            catch (Exception)
            {
            }
        }
        public void AddListForAdmin(int nottypeid, string nottitle, IQueryable<Admin> list)
        {
            try
            {
                foreach (var item in list)
                {
                    //Add Notification
                    Notification notification = new Notification
                    {
                        Enabled = false,
                        NotTypeId = nottypeid,
                        SendDate = DateTime.Now,
                        Title = nottitle,
                        UserId = Convert.ToInt32(item.Id),
                        UserRole = "admin"
                    };
                    _db.Notification.Add(notification);
                }
                _db.SaveChanges();
            }
            catch (Exception)
            {
            }
        }
        
        public void AddListForCurrent(int nottypeid, string nottitle, IQueryable<Current> list)
        {
            try
            {
                foreach (var item in list)
                {
                    //Add Notification
                    Notification notification = new Notification
                    {
                        Enabled = false,
                        NotTypeId = nottypeid,
                        SendDate = DateTime.Now,
                        Title = nottitle,
                        UserId = Convert.ToInt32(item.Id),
                        UserRole = "current"
                    };
                    _db.Notification.Add(notification);
                }
                _db.SaveChanges();
            }
            catch (Exception)
            {
            }
        }
    }
}
