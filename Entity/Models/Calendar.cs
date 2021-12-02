using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Calendar
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string BackgroundColor { get; set; }
        public string BorderColor { get; set; }
        public bool Status { get; set; }
        public int Type { get; set; }
        public bool AllDay { get; set; }
        public int? UserId { get; set; }
        public string UserRole { get; set; }
    }
}
