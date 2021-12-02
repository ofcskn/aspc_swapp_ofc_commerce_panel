using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class CurrentSaleProcess
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public string Mail { get; set; }
        public int? SaleProcessId { get; set; }
        public DateTime? Date { get; set; }
        public int? Amount { get; set; }
        public decimal? Price { get; set; }
        public string Total { get; set; }
    }
}
