
using Entity.Context;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Concrete.EntityFramework
{
    public class EfEmailStatusService : EfGenericService<EmailStatus>, IEmailStatusService
    {
        public EfEmailStatusService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public EmailStatus AddEmailStatus(int emailId, int userId, string userRole)
        {
            EmailStatus es = new EmailStatus
            {
                EmailId = emailId,
                UserId = userId,
                UserRole = userRole
            };
            Add(es);
            return es;
        }
        public EmailStatus GetEmailStatus(int emailId, string userRole, int userId)
        {
            return _db.EmailStatus.FirstOrDefault(p => p.EmailId == emailId && p.UserId == userId && p.UserRole == userRole);
        }
        public EmailStatus GetEmailStatusWithEmail(int emailId, string userRole, int userId)
        {
            return _db.EmailStatus.Include(p => p.Email).FirstOrDefault(p => p.EmailId == emailId && p.UserId == userId && p.UserRole == userRole);
        }
        public bool ChangeStatus(int emailId, string userRole, int userId, string filter, bool status)
        {
            EmailStatus es = GetEmailStatus(emailId, userRole, userId);
            if (es == null)
            {
                var email = _db.Email.FirstOrDefault(p => p.Id == emailId);
                es = AddEmailStatus(emailId, userId, userRole);
            }
            if (filter == "favourite")
            {
                es.FavouriteStatus = status;
                es.FavouriteDate = DateTime.Now;
            }
            else if (filter == "junk")
            {
                es.JunkStatus = status;
                es.JunkDate = DateTime.Now;
            }
            else if (filter == "trash")
            {
                es.TrashStatus = status;
                es.TrashDate = DateTime.Now;
            }
            else if (filter == "permanent-trash")
            {
                es.PermanentStatus = status;
                es.PermanentDate = DateTime.Now;
            }
            else if (filter == "read")
            {
                es.ReadStatus = status;
                es.ReadDate = DateTime.Now;
            }
            Update(es);
            return status;

        }

    }
}
