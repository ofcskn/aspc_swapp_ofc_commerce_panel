using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class CargoProcess
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int ProductCargoId { get; set; }
        public bool Enabled { get; set; }
        public string Description { get; set; }

        public virtual ProductCargo ProductCargo { get; set; }
    }
}
