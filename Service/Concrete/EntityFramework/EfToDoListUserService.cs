using Entity.Context;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Concrete.EntityFramework
{
    public class EfToDoListUserService : EfGenericService<ToDoListUser>, IToDoListUserService
    {
        public EfToDoListUserService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }

        public IQueryable<ToDoListUser> GetAllByGroupId(int groupId, bool status)
        {
            return _db.ToDoListUser.Where(p => 
            p.GroupId == groupId && p.Enabled == status).OrderByDescending(p=>p.Date);
        }
        public IQueryable<ToDoListUser> GetAllByGroupId(int groupId)
        {
            var list = _db.ToDoListUser.Where(p => p.GroupId == groupId).OrderByDescending(p => p.Date);
            return list;
        }
    }
}
