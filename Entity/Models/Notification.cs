using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime SendDate { get; set; }
        public bool? Enabled { get; set; }
        public DateTime? ReadDate { get; set; }
        public int UserId { get; set; }
        public string UserRole { get; set; }
        public int NotTypeId { get; set; }
        public bool ReadEnabled { get; set; }

        public virtual NotificationType NotType { get; set; }
    }
}
