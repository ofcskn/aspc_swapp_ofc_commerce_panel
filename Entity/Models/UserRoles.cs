using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class UserRoles
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public bool Enabled { get; set; }
    }
}
