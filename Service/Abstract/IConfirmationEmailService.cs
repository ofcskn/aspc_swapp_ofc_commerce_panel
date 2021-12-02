using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IConfirmationEmailService : IGenericService<ConfirmationEmail>
    {
         string UpdateUserStatus(ConfirmationEmail entity);
        string ConfirmSubscriberControl(string confirmLink, string readLink);
    }
}
