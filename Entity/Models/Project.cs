using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Priority { get; set; }
        public bool Enabled { get; set; }
        public string Link { get; set; }
        public DateTime? Date { get; set; }
    }
}
