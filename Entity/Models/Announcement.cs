using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Announcement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime Date { get; set; }
        public int AnnouncerId { get; set; }
        public string AnnouncerRole { get; set; }
        public int ReadNumber { get; set; }
        public bool Enabled { get; set; }
    }
}
