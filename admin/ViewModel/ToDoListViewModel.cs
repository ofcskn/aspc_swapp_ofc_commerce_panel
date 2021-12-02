using admin.Models;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admin.ViewModel
{
    public class ToDoListViewModel
    {
        public PaginatedList<ToDoListGroup> PToDoListGroups { get; set; }
        public PaginatedList<ToDoList> PToDoLists { get; set; }
        public PaginatedList<ToDoListUser> PToDoListUsers { get; set; }
        public IQueryable<ToDoList> ToDoLists { get; set; }
        public IQueryable<ToDoListGroup> ToDoListGroups { get; set; }
        public IQueryable<ToDoListUser> ToDoListUsers { get; set; }

        public IQueryable<UserViewModel> Users { get; set; }

        public ToDoListUser ToDoListUser { get; set; }
        public ToDoListGroup ToDoListGroup { get; set; }
        public ToDoList ToDoList { get; set; }
        public string GroupId { get; set; }
        public string AdminName { get; set; }
    }
}
