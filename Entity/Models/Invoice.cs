using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceDetail = new HashSet<InvoiceDetail>();
            SaleProcess = new HashSet<SaleProcess>();
        }

        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public DateTime SendDate { get; set; }
        public string TaxAdministration { get; set; }
        public int StaffId { get; set; }
        public int? CurrentId { get; set; }
        public DateTime? DueDate { get; set; }

        public virtual Current Current { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetail { get; set; }
        public virtual ICollection<SaleProcess> SaleProcess { get; set; }
    }
}
