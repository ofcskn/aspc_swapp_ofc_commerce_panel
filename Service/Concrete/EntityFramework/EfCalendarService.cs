
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
    public class EfCalendarService : EfGenericService<Calendar>, ICalendarService
    {
        public EfCalendarService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public IQueryable FindAllEvents(int userId, string userRole)
        {
            var events = _db.Calendar.Where(p=>p.UserRole == userRole && p.UserId == userId).Select(p => new
            {
                id = p.Id,
                title = p.Title,
                allDay = p.AllDay,
                backgroundColor = p.BackgroundColor,
                borderColor = p.BorderColor,
                start = p.StartDate,
                end = p.EndDate,
            });
            return events;
        }
    }
}
