using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class ToDoListUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserRole { get; set; }
        public int GroupId { get; set; }
        public DateTime? Date { get; set; }
        public bool? Enabled { get; set; }

        public virtual ToDoListGroup Group { get; set; }
    }
}
