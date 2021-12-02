using Entity.Context;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Service.Abstract;
using Service.Utilities;

namespace Service.Concrete.EntityFramework
{
    public class EfConfirmationEmailService : EfGenericService<ConfirmationEmail>, IConfirmationEmailService
    {
        public EfConfirmationEmailService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public string ConfirmSubscriberControl(string confirmLink, string readLink)
        {
            if (readLink == confirmLink)
            {
                return "";
            }
            else
            {
                return null;
            }
        }
        public string UpdateUserStatus(ConfirmationEmail entity)
        {
            var admin = _db.Admin.FirstOrDefault(p => p.Id == entity.AdminId);
            admin.Enabled = true;
            _db.Admin.Update(admin);

            var current = _db.Current.FirstOrDefault(p => p.Id == entity.CurrentId);
            current.Status = true;
            _db.Current.Update(current);

            var staff = _db.Staff.FirstOrDefault(p => p.Id == entity.StaffId);
            staff.Status = true;
            _db.Staff.Update(staff);

            entity.ReadDate = DateTime.Now;
            _db.ConfirmationEmail.Update(entity);

            _db.SaveChanges();

            return admin.Name + " " + admin.Surname;
        }
    }
}
