
using Entity.Context;
using Entity.Models;
using Entity.ViewModel;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Concrete.EntityFramework
{
    public class EfEmailService : EfGenericService<Email>, IEmailService
    {
        public EfEmailService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public IQueryable<Email> GetAllReceivedMail(int receiverId, string role)
        {
            return _db.EmailStatus.Include(p => p.Email)
                .Where(p => p.PermanentStatus == false && p.TrashStatus == false
                && p.Email.ReceiverId == receiverId && p.Email.ReceiverRole == role && p.UserId == receiverId && p.UserRole == role
                && p.Email.DraftEnabled == false).OrderByDescending(p => p.Email.SendDate)
                 .Select(p => new Email
                 {
                     Description = p.Email.Description,
                     Subject = p.Email.Subject,
                     DraftDate = p.Email.DraftDate,
                     Id = p.Email.Id,
                     SendDate = p.Email.SendDate,
                     EmailStatus = p.Email.EmailStatus,
                     EmailAttachments = p.Email.EmailAttachments,
                     SenderId = p.Email.SenderId,
                     SenderRole = p.Email.SenderRole,
                     DraftEnabled = p.Email.DraftEnabled,
                     ReceiverId = p.Email.ReceiverId,
                     ReceiverRole = p.Email.ReceiverRole,
                     SenderName = p.Email.SenderName
                 }); ;
        }
        public IQueryable<Email> GetAllSendedMail(int senderId, string role)
        {
            return _db.EmailStatus.Include(p => p.Email)
                .Where(p => p.Email.DraftEnabled == false && p.PermanentStatus == false && p.TrashStatus == false
                && p.UserId == senderId && p.UserRole == role && p.Email.SenderId == senderId && p.Email.SenderRole == role)
                .OrderByDescending(p => p.Email.SendDate)
                 .Select(p => new Email
                 {
                     Description = p.Email.Description,
                     Subject = p.Email.Subject,
                     DraftDate = p.Email.DraftDate,
                     Id = p.Email.Id,
                     SendDate = p.Email.SendDate,
                     EmailStatus = p.Email.EmailStatus,
                     EmailAttachments = p.Email.EmailAttachments,
                     SenderId = p.Email.SenderId,
                     SenderRole = p.Email.SenderRole,
                     DraftEnabled = p.Email.DraftEnabled,
                     ReceiverId = p.Email.ReceiverId,
                     ReceiverRole = p.Email.ReceiverRole,
                     SenderName = p.Email.SenderName
                 });
        }

        public IQueryable<Email> GetAllReceivedByRead(int userId, string role, bool readStatus)
        {
            return _db.EmailStatus.Include(p => p.Email)
               .Where(p => p.PermanentStatus == false && p.TrashStatus == false
               && p.Email.ReceiverId == userId && p.Email.ReceiverRole == role && p.UserId == userId && p.UserRole == role
               && p.Email.DraftEnabled == false && p.ReadStatus == readStatus).OrderByDescending(p => p.Email.SendDate)
                .Select(p => new Email
                {
                    Description = p.Email.Description,
                    Subject = p.Email.Subject,
                    DraftDate = p.Email.DraftDate,
                    Id = p.Email.Id,
                    SendDate = p.Email.SendDate,
                    EmailStatus = p.Email.EmailStatus,
                    EmailAttachments = p.Email.EmailAttachments,
                    SenderId = p.Email.SenderId,
                    SenderRole = p.Email.SenderRole,
                    DraftEnabled = p.Email.DraftEnabled,
                    ReceiverId = p.Email.ReceiverId,
                    ReceiverRole = p.Email.ReceiverRole,
                    SenderName = p.Email.SenderName
                });
        }

        public IQueryable<Email> GetEmailResultBySearch(string q, IQueryable<Email> list)
        {
            IQueryable<Email> filtered = list.AsQueryable();
            if (!string.IsNullOrEmpty(q) && filtered.Count() > 0)
            {
                filtered = filtered.Where(p => p.Subject.Contains(q)
                || p.Description.Contains(q));
            }
            return filtered;
        }
        public string GetReceiverInfoByEmail(string receiverEmail, string filter)
        {
            var admin = _db.Admin.FirstOrDefault(p => p.Email == receiverEmail);
            var current = _db.Current.FirstOrDefault(p => p.Mail == receiverEmail);
            var staff = _db.Staff.FirstOrDefault(p => p.Email == receiverEmail);

            if (filter == "id")
            {
                int receiverId = 0;
                if (admin != null)
                {
                    receiverId = admin.Id;
                }
                if (current != null)
                {
                    receiverId = current.Id;
                }
                if (staff != null)
                {
                    receiverId = staff.Id;
                }
                return receiverId.ToString();
            }
            if (filter == "role")
            {
                string recinf = null;
                if (admin != null)
                {
                    recinf = admin.Role;
                }
                if (current != null)
                {
                    recinf = "current";
                }
                if (staff != null)
                {
                    recinf = "staff";
                }
                return recinf;
            }
            return null;
        }
        public IQueryable<Email> GetAllFavouriteMail(int userId, string userRole)
        {
            IQueryable<Email> list = _db.EmailStatus
                .Where(p => p.UserId == userId && p.UserRole == userRole && p.FavouriteStatus == true && p.TrashStatus == false && p.PermanentStatus == false).Include(p => p.Email)
              //.Select(p => p.Email);
              .Select(p => new Email
              {
                  Description = p.Email.Description,
                  Subject = p.Email.Subject,
                  DraftDate = p.Email.DraftDate,
                  Id = p.Email.Id,
                  SendDate = p.Email.SendDate,
                  EmailStatus = p.Email.EmailStatus,
                  EmailAttachments = p.Email.EmailAttachments,
                  SenderId = p.Email.SenderId,
                  SenderRole = p.Email.SenderRole,
                  DraftEnabled = p.Email.DraftEnabled,
                  ReceiverId = p.Email.ReceiverId,
                  ReceiverRole = p.Email.ReceiverRole,
                  SenderName = p.Email.SenderName
              });
            return list;
        }
        public IQueryable<Email> GetAllDraftMail(int senderId, string role)
        {
            return _db.EmailStatus.Include(p => p.Email)
                 .Where(p => p.UserId == senderId && p.UserRole == role
                  && p.Email.DraftEnabled == true && p.PermanentStatus == false && p.TrashStatus == false)
                 .OrderByDescending(p => p.Email.SendDate)
                 .Select(p => new Email
                 {
                     Description = p.Email.Description,
                     Subject = p.Email.Subject,
                     DraftDate = p.Email.DraftDate,
                     Id = p.Email.Id,
                     SendDate = p.Email.SendDate,
                     EmailStatus = p.Email.EmailStatus,
                     EmailAttachments = p.Email.EmailAttachments,
                     SenderId = p.Email.SenderId,
                     SenderRole = p.Email.SenderRole,
                     DraftEnabled = p.Email.DraftEnabled,
                     ReceiverId = p.Email.ReceiverId,
                     ReceiverRole = p.Email.ReceiverRole,
                     SenderName = p.Email.SenderName
                 });
        }
        public IQueryable<Email> GetAllRubbishMail(int userId, string role)
        {
            return _db.EmailStatus.Include(p => p.Email)
                .Where(p => (p.UserId == userId && p.UserRole == role)
                && p.TrashStatus == true && p.PermanentStatus == false)
                .OrderByDescending(p => p.Email.SendDate)
                  .Select(p => new Email
                  {
                      Description = p.Email.Description,
                      Subject = p.Email.Subject,
                      DraftDate = p.Email.DraftDate,
                      Id = p.Email.Id,
                      SendDate = p.Email.SendDate,
                      EmailStatus = p.Email.EmailStatus,
                      EmailAttachments = p.Email.EmailAttachments,
                      SenderId = p.Email.SenderId,
                      SenderRole = p.Email.SenderRole,
                      DraftEnabled = p.Email.DraftEnabled,
                      ReceiverId = p.Email.ReceiverId,
                      ReceiverRole = p.Email.ReceiverRole,
                      SenderName = p.Email.SenderName
                  });
        }
        public Email GetEmail(int id)
        {
            return _db.Email.Include(p => p.EmailStatus).Include(p => p.EmailAttachments).FirstOrDefault(p => p.Id == id);
        }
        public void ComposeMail(Email entity, int userId, string userRole)
        {
            EmailStatus es = new EmailStatus
            {
                EmailId = entity.Id,
                UserId = userId,
                UserRole = userRole
            };
            _db.Email.Add(entity);
            _db.EmailStatus.Add(es);
            _db.SaveChanges();
        }

    }
}
