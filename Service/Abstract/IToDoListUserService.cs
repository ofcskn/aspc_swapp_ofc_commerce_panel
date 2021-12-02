using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IToDoListUserService : IGenericService<ToDoListUser>
    {
        IQueryable<ToDoListUser> GetAllByGroupId(int groupId, bool status);
         IQueryable<ToDoListUser> GetAllByGroupId(int groupId);
    }
}
