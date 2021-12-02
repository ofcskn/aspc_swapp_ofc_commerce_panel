using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public DateTime Date { get; set; }
        public int Enabled { get; set; }
    }
}
