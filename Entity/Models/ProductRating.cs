using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class ProductRating
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RatingScore { get; set; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
