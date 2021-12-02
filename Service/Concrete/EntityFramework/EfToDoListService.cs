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
    public class EfToDoListService : EfGenericService<ToDoList>, IToDoListService
    {
        IHttpContextAccessor _httpContextAccessor;
        public EfToDoListService(SwappDbContext _context, IHttpContextAccessor httpContextAccessor) : base(_context)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public string GetRoleByAdmin
        {
            get { return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "role").Value; }
        }

        public string GetIdByAdmin
        {
            get { return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "id").Value; }
        }

        public IQueryable<ToDoList> GetAllByEnabled(int userId, string userRole, bool status)
        {
            return _db.ToDoList.Where(p => p.MemberId == userId && p.Role == userRole && p.Enabled == status).OrderByDescending(p => p.StartDate);
        }
        public IQueryable<ToDoList> GetAllByGroupId(int groupId)
        {
            var list = _db.ToDoList.Where(p => p.GroupId == groupId).Include(p=>p.Group).Include(p=>p.Group.ToDoListUser).OrderByDescending(p => p.StartDate);
            return list;
        }
        public IQueryable<ToDoList> GetAllByGroupId(int groupId, bool status)
        {
            var list = _db.ToDoList.Where(p => p.Enabled == status  &&  p.GroupId == groupId).Include(p=>p.Group).Include(p=>p.Group.ToDoListUser).OrderByDescending(p => p.StartDate);
            return list;
        }
        public IQueryable<ToDoList> GetAllByUser(int userId, string userRole, bool status)
        {
            var list = _db.ToDoList.Where(p => p.Enabled == status && p.Role == userRole&& p.MemberId == userId).OrderByDescending(p => p.StartDate);
            return list;
        }
        public IQueryable<ToDoList> GetAllByUser(int userId, string userRole)
        {
            var list = _db.ToDoList.Where(p => p.Role == userRole&& p.MemberId == userId).OrderByDescending(p => p.StartDate);
            return list;
        }
    }
}
