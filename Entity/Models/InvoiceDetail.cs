using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class InvoiceDetail
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }

        public virtual Invoice Invoice { get; set; }
    }
}
