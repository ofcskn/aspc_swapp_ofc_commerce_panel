using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class NotificationType
    {
        public NotificationType()
        {
            Notification = new HashSet<Notification>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string GeneralType { get; set; }

        public virtual ICollection<Notification> Notification { get; set; }
    }
}
