using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class ProductDescription
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
