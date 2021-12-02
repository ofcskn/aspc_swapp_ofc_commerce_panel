using Entity.Context;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Concrete.EntityFramework
{
    public class EfToDoListGroupService : EfGenericService<ToDoListGroup>, IToDoListGroupService
    {
        public EfToDoListGroupService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public IQueryable<ToDoListGroup> GetAllByEnabled(int userId, string userRole, bool status)
        {
            return _db.ToDoListGroup.Where(p => p.Enabled == false
            //&& p.MemberId == userId && p.Role == userRole 
            && p.Enabled == status).OrderByDescending(p => p.Date);
        }
        public IQueryable<ToDoListGroup> GetAllByDateGroup(int userId, string userRole)
        {
            var list = _db.ToDoListGroup.Where(p => p.Enabled == false
            //&& p.MemberId == userId && p.Role == userRole 
            ).OrderByDescending(p => p.Date);
            return list;
        }
        public IQueryable<ToDoListGroup> GetAllByGroupId(bool byPriority, int groupId)
        {
            if (byPriority == true)
            {
                var list = _db.ToDoListGroup.Where(p => p.GroupId == groupId)
                .Include(p => p.ToDoListUser)
                .OrderBy(p => p.Priority);
                return list;
            }
            else
            {
                var list = _db.ToDoListGroup.Where(p => p.GroupId == groupId)
                  .Include(p => p.ToDoListUser);
                return list;
            }
        }
        public ToDoListGroup GetByIdWithInclude(int groupId)
        {
            return _db.ToDoListGroup.Include(p=>p.ToDoList).Include(p=>p.ToDoListUser).FirstOrDefault(p => p.Id == groupId);
        }
        public IQueryable<ToDoListGroup> GetAllByUser(int userId, string userRole, bool byPriority)
        {

            if (byPriority == true)
            {
                var list = _db.ToDoListUser.Where(p => p.UserId == userId && p.UserRole == userRole)
                .Include(p => p.Group).Include(p => p.Group.ToDoListUser)
                .Select(p => p.Group).Where(p => p.GroupId == null)
                .OrderBy(p => p.Priority);
                return list;
            }
            else
            {
                var list = _db.ToDoListUser.Where(p => p.UserId == userId && p.UserRole == userRole)
                .Include(p => p.Group).Include(p => p.Group.ToDoListUser)
                .Select(p => p.Group).Where(p => p.GroupId == null);
                return list;
            }
        }
        public IQueryable<ToDoListGroup> GetAllByUserByEnabled(int userId, string userRole, bool status, bool byPriority)
        {

            if (byPriority == true)
            {
                var list = _db.ToDoListUser.Where(p => p.UserId == userId && p.UserRole == userRole && p.Enabled == status)
                .Include(p => p.Group).Include(p => p.Group.ToDoListUser)
                .Select(p => p.Group).Where(p => p.GroupId == null)
                .OrderBy(p => p.Priority);
                return list;
            }
            else
            {
                var list = _db.ToDoListUser.Where(p => p.UserId == userId && p.UserRole == userRole && p.Enabled == status)
                .Include(p => p.Group).Include(p => p.Group.ToDoListUser)
                .Select(p => p.Group).Where(p => p.GroupId == null);
                return list;
            }
        }
    }
}
