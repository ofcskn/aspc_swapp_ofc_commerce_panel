
using Entity.Context;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;
using Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Concrete.EntityFramework
{
    public class EfStaffService : EfGenericService<Staff>, IStaffService
    {
        public EfStaffService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public Staff GetStaff(string username, string password)
        {
            var encryptedText = Cipher.Encrypt(password, username);
            return _db.Staff.FirstOrDefault(p => p.UserName == username && p.Password == encryptedText);
        }
        public IQueryable<Staff> GetAllByDepartment(int depId)
        {
            return _db.Staff.Include(p=>p.Department).Where(p => p.DepartmentId == depId).OrderBy(p=>p.Priority);
        }
        public IQueryable<Staff> GetAllByDate()
        {
            return _db.Staff.Include(p => p.Department).OrderByDescending(p=>p.StartDate).OrderBy(p => p.Priority);
        }
        public IQueryable<Staff> GetAllByEnabled()
        {
            return _db.Staff.Include(p => p.Department).OrderBy(p => p.Status == true);
        }
    }
}
