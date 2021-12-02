using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Current
    {
        public Current()
        {
            Invoice = new HashSet<Invoice>();
            ProductCargo = new HashSet<ProductCargo>();
        }

        public int Id { get; set; }
        public string No { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public string Mail { get; set; }
        public bool Status { get; set; }
        public string Password { get; set; }
        public DateTime RegisterDate { get; set; }
        public string RegisterIp { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string LastLoginIp { get; set; }
        public string Image { get; set; }
        public int? PinCode { get; set; }

        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<ProductCargo> ProductCargo { get; set; }
    }
}
