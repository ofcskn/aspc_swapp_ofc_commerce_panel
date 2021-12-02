using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class ToDoList
    {
        public int Id { get; set; }
        public string Goal { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Enabled { get; set; }
        public int MemberId { get; set; }
        public string Role { get; set; }
        public int? GroupId { get; set; }

        public virtual ToDoListGroup Group { get; set; }
    }
}
