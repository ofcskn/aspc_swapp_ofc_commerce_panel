using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class TimeLine
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Lang { get; set; }
        public int MemberId { get; set; }
        public string MemberRole { get; set; }
        public int? TlekstraId { get; set; }
        public bool? IsAll { get; set; }

        public virtual TimeLineEkstra Tlekstra { get; set; }
    }
}
