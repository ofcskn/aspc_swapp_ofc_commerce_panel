
using Entity.Context;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Concrete.EntityFramework
{
    public class EfTimeLineService : EfGenericService<TimeLine>, ITimeLineService
    {
        public EfTimeLineService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public IQueryable<TimeLine> GetAllTL()
        {
            return _db.TimeLine.Include(p=>p.Tlekstra);
        }
        public IQueryable<TimeLineEkstra> GetAllTLE()
        {
            return _db.TimeLineEkstra;
        }
        public void AddTL(TimeLineEkstra entity)
        {
            _db.TimeLineEkstra.Add(entity);
            _db.SaveChanges();
        }
    }
}
