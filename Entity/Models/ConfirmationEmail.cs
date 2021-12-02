using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class ConfirmationEmail
    {
        public int Id { get; set; }
        public int ConfirmId { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public int AdminId { get; set; }
        public int CurrentId { get; set; }
        public int StaffId { get; set; }
        public string AdminEmail { get; set; }
        public string ConfirmationCode { get; set; }
    }
}
