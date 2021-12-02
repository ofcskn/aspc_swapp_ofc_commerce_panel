using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IToDoListGroupService : IGenericService<ToDoListGroup>
    {
        IQueryable<ToDoListGroup> GetAllByDateGroup(int userId, string userRole);
        IQueryable<ToDoListGroup> GetAllByEnabled(int userId, string userRole, bool status);
        IQueryable<ToDoListGroup> GetAllByUser(int userId, string userRole,  bool byPriority);
        IQueryable<ToDoListGroup> GetAllByUserByEnabled(int userId, string userRole, bool status, bool byPriority);
        IQueryable<ToDoListGroup> GetAllByGroupId(bool byPriority, int groupId);
        ToDoListGroup GetByIdWithInclude(int groupId);
    }
}
