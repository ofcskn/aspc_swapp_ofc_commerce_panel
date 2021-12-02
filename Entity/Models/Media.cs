using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Media
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Folder { get; set; }
        public string FileNames { get; set; }
        public DateTime Date { get; set; }
        public string Path { get; set; }
        public bool? Enabled { get; set; }
        public string Project { get; set; }
    }
}
