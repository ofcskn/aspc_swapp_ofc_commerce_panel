using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Comment
    {
        public Comment()
        {
            ProductComment = new HashSet<ProductComment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        public int CommentId { get; set; }
        public bool Enabled { get; set; }

        public virtual ICollection<ProductComment> ProductComment { get; set; }
    }
}
