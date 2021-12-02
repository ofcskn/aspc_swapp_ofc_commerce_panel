using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Department
    {
        public Department()
        {
            Staff = new HashSet<Staff>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<Staff> Staff { get; set; }
    }
}
