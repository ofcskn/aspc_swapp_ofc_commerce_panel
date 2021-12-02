using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Total { get; set; }
    }
}
