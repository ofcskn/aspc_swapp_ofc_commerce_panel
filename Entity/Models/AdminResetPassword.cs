using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class AdminResetPassword
    {
        public int Id { get; set; }
        public int? AdminId { get; set; }
        public bool? Enabled { get; set; }
        public DateTime? ResetDate { get; set; }
        public DateTime? ExpireTime { get; set; }
        public DateTime? SendDate { get; set; }
        public string Ip { get; set; }
    }
}
