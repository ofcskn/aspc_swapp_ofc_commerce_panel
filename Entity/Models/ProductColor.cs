using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class ProductColor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ColorClass { get; set; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
