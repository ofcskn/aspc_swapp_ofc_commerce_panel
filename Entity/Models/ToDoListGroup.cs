using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class ToDoListGroup
    {
        public ToDoListGroup()
        {
            ToDoList = new HashSet<ToDoList>();
            ToDoListUser = new HashSet<ToDoListUser>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string TitleColor { get; set; }
        public DateTime Date { get; set; }
        public string Image { get; set; }
        public int Priority { get; set; }
        public bool Enabled { get; set; }
        public string Icon { get; set; }
        public int? GroupId { get; set; }
        public int AdminId { get; set; }
        public string AdminRole { get; set; }

        public virtual ICollection<ToDoList> ToDoList { get; set; }
        public virtual ICollection<ToDoListUser> ToDoListUser { get; set; }
    }
}
