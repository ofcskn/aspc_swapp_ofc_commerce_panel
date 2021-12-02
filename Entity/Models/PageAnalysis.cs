using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class PageAnalysis
    {
        public int Id { get; set; }
        public DateTime EntryDate { get; set; }
        public string Ip { get; set; }
        public string Page { get; set; }
        public DateTime? EndDate { get; set; }
        public string Lang { get; set; }
        public string Browser { get; set; }
    }
}
