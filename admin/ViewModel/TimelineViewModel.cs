using admin.Models;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admin.ViewModel
{
    public class TimelineViewModel
    {
        public List<TimelineModel> Timelines { get; set; }
        public TimeLine Timeline { get; set; }
        public TimeLineEkstra TimeLineEkstra { get; set; }
    }
}
