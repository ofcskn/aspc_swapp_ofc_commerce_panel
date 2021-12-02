using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class ProductCargo
    {
        public ProductCargo()
        {
            CargoProcess = new HashSet<CargoProcess>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CurrentId { get; set; }
        public bool Enabled { get; set; }
        public string CargoNo { get; set; }
        public string QrCode { get; set; }
        public string CargoChaseLink { get; set; }
        public int CargoCompanyId { get; set; }
        public int ProductId { get; set; }

        public virtual CargoCompany CargoCompany { get; set; }
        public virtual Current Current { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<CargoProcess> CargoProcess { get; set; }
    }
}
