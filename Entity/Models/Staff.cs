using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Staff
    {
        public Staff()
        {
            Invoice = new HashSet<Invoice>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? DepartmentId { get; set; }
        public bool Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int? Priority { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string LastLoginIp { get; set; }
        public int? PinCode { get; set; }
        public string Image { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
    }
}
