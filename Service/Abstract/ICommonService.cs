using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Abstract
{
    public interface ICommonService
    {
        string SendMailToDefault(string subject, string bodyHTML, string bodyText, IConfiguration Configuration);
        string SendMailMultipleWithMail( string subject, string bodyHTML, string bodyText, IConfiguration Configuration, List<string> toMails, IEnumerable<IFormFile> Files);
        string SendMailToAnother(string subject, string bodyHTML, string bodyText, IConfiguration Configuration, string toEmail);
    }
}
