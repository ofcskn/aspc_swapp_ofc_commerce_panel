using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Language
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public bool IsActive { get; set; }
    }
}
