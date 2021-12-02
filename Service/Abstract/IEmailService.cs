using Entity.Models;
using Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IEmailService : IGenericService<Email>
    {
        IQueryable<Email> GetAllReceivedMail(int receiverId, string role);
        IQueryable<Email> GetAllSendedMail(int senderId, string role);
        IQueryable<Email> GetAllFavouriteMail(int userId, string userRole);
        IQueryable<Email> GetAllDraftMail(int receiverId, string role);
        IQueryable<Email> GetAllRubbishMail(int receiverId, string role);
        IQueryable<Email> GetAllReceivedByRead(int receiverId, string role, bool readStatus);
        string GetReceiverInfoByEmail(string receiverEmail, string filter);
        Email GetEmail(int id);
        void ComposeMail(Email entity, int userId, string userRole);
        IQueryable<Email> GetEmailResultBySearch(string q, IQueryable<Email> list);
    }
}
