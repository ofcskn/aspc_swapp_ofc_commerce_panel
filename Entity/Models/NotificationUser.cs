using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class NotificationUser
    {
        public int Id { get; set; }
        public bool AllEnabled { get; set; }
        public int UserId { get; set; }
        public string UserRole { get; set; }
    }
}
