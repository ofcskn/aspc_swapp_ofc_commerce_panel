using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class ProductSize
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public string ShortSize { get; set; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
