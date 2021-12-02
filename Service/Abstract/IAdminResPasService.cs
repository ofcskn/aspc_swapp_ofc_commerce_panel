using Entity.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IAdminResPasService : IGenericService<AdminResetPassword>
    {
        public string SendResetPasswordEmail(string subject, string bodyHTML, string bodyText, IConfiguration Configuration, Admin admin, string username, string email, string password);
        string GetUserIp { get; }
    }
}
