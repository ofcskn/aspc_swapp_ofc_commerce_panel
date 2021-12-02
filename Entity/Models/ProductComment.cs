using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class ProductComment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        public int? CommentId { get; set; }
        public bool Enabled { get; set; }

        public virtual Product Product { get; set; }
    }
}
