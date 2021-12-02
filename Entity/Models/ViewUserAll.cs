using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class ViewUserAll
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public string Mail { get; set; }
        public string Role { get; set; }
    }
}
