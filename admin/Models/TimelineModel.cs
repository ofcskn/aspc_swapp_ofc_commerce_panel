using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admin.Models
{
    public class TimelineModel
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

        public string MemberNameSurname { get; set; }
        public string Avatar { get; set; }


        public string ColorCode { get; set; }
        public string IconClass { get; set; }
    }
}
