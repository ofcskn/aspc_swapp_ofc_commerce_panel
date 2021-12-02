using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class MessageRooms
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AdminId { get; set; }
        public string AdminRole { get; set; }
        public bool? Enabled { get; set; }
        public string GroupFor { get; set; }
    }
}
