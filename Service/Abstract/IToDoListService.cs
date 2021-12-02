using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IToDoListService : IGenericService<ToDoList>
    {
        string GetRoleByAdmin { get; }
        string GetIdByAdmin { get; }
        IQueryable<ToDoList> GetAllByEnabled(int userId, string userRole, bool status);
        IQueryable<ToDoList> GetAllByGroupId(int groupId);
        IQueryable<ToDoList> GetAllByGroupId(int groupId, bool status);
        IQueryable<ToDoList> GetAllByUser(int userId, string userRole);
        IQueryable<ToDoList> GetAllByUser(int userId, string userRole, bool status);
    }
}
