using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Search
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Permalink { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public DateTime Date { get; set; }
    }
}
