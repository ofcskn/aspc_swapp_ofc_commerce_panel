using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class RememberPassword
    {
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public int? PinCode { get; set; }
    }
}
