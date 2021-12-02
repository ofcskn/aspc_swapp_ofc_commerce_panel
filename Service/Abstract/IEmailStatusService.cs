using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IEmailStatusService : IGenericService<EmailStatus>
    {
         EmailStatus AddEmailStatus(int emailId, int userId, string userRole);
        bool ChangeStatus(int emailId, string userRole, int userId, string filter, bool status);
        EmailStatus GetEmailStatus(int emailId, string userRole, int userId);
        EmailStatus GetEmailStatusWithEmail(int emailId, string userRole, int userId);
    }
}
