using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface ITimeLineService : IGenericService<TimeLine>
    {
         void AddTL(TimeLineEkstra entity);
        IQueryable<TimeLine> GetAllTL();
        IQueryable<TimeLineEkstra> GetAllTLE();
    }
}
