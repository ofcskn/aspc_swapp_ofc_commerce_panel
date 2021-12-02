
using Entity.Context;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;
using Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Concrete.EntityFramework
{
    public class EfCurrentService : EfGenericService<Current>, ICurrentService
    {
        public EfCurrentService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }

        public Current GetByNo(string memberNo)
        {
            return _db.Current.FirstOrDefault(p=>p.No == memberNo);
        }
        public Current GetByIdCurrent(int id)
        {
            return _db.Current.Include(p=>p.ProductCargo).FirstOrDefault(p=>p.Id == id);
        }
        public bool ControlMembership(string memberNo)
        {
            if (_db.Current.FirstOrDefault(p => p.No == memberNo) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Current GetCurrent(string username, string password)
        {
            var encryptedText = Cipher.Encrypt(password, username);
            return _db.Current.FirstOrDefault(p => p.UserName == username && p.Password == encryptedText);
        }
        public IQueryable<Current> GetAllByEnabled(bool status)
        {
            return _db.Current.Where(p=>p.Status == status);
        }
    }
}
