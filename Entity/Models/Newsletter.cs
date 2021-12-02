using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Newsletter
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public DateTime Date { get; set; }
        public string Ip { get; set; }
        public bool? Enabled { get; set; }
        public DateTime? UnsubscribeDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public bool? ReadEnabled { get; set; }
    }
}
