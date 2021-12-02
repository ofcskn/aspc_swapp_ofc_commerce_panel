using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class CargoCompany
    {
        public CargoCompany()
        {
            ProductCargo = new HashSet<ProductCargo>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string WebSite { get; set; }

        public virtual ICollection<ProductCargo> ProductCargo { get; set; }
    }
}
