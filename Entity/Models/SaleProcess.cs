using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class SaleProcess
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Amount { get; set; }
        public string Price { get; set; }
        public string Total { get; set; }
        public int? StaffId { get; set; }
        public int? CurrentId { get; set; }
        public int ProductId { get; set; }
        public bool Status { get; set; }
        public int? InvoiceId { get; set; }

        public virtual Invoice Invoice { get; set; }
    }
}
