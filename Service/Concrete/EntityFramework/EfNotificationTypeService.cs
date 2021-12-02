
using Entity.Context;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Concrete.EntityFramework
{
    public class EfNotificationTypeService : EfGenericService<NotificationType>, INotificationTypeService
    {
        public EfNotificationTypeService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public NotificationType GetNTByType(string type)
        {
            return _db.NotificationType.Include(p=>p.Notification).FirstOrDefault(p=>p.Name == type);
        }
    }
}
