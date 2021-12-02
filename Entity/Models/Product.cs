using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductCargo = new HashSet<ProductCargo>();
            ProductColor = new HashSet<ProductColor>();
            ProductComment = new HashSet<ProductComment>();
            ProductDescription = new HashSet<ProductDescription>();
            ProductImage = new HashSet<ProductImage>();
            ProductRating = new HashSet<ProductRating>();
            ProductSize = new HashSet<ProductSize>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Stock { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SalePrice { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
        public bool Status { get; set; }
        public string Barcode { get; set; }
        public DateTime Date { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<ProductCargo> ProductCargo { get; set; }
        public virtual ICollection<ProductColor> ProductColor { get; set; }
        public virtual ICollection<ProductComment> ProductComment { get; set; }
        public virtual ICollection<ProductDescription> ProductDescription { get; set; }
        public virtual ICollection<ProductImage> ProductImage { get; set; }
        public virtual ICollection<ProductRating> ProductRating { get; set; }
        public virtual ICollection<ProductSize> ProductSize { get; set; }
    }
}
