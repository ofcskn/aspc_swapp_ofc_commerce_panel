using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class TimeLineEkstra
    {
        public TimeLineEkstra()
        {
            TimeLine = new HashSet<TimeLine>();
        }

        public int Id { get; set; }
        public string ColorCode { get; set; }
        public string IconClass { get; set; }

        public virtual ICollection<TimeLine> TimeLine { get; set; }
    }
}
