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
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MailKit.Security;

namespace Service.Concrete.EntityFramework
{
    public class EfAdminResPasService : EfGenericService<AdminResetPassword>, IAdminResPasService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EfAdminResPasService(SwappDbContext _context, IHttpContextAccessor httpContextAccessor) : base(_context)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public string GetUserIp
        {
            get { return _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(); }
        }
        public string SendResetPasswordEmail(string subject, string bodyHTML, string bodyText, IConfiguration Configuration, Admin admin, string username, string toMail, string passwordChangeLink)
        {
            try
            {
                string userName = admin.Name + admin.Surname;

                MimeMessage message = new MimeMessage();
                string fromName = Configuration.GetSection("Email").GetSection("DisplayName").Value;
                string fromMail = Configuration.GetSection("Email").GetSection("From").Value;
                string fromPassword = Configuration.GetSection("Email").GetSection("Password").Value;
                string smtpPort = Configuration.GetSection("Email").GetSection("SmtpPort").Value;
                string smtpServer = Configuration.GetSection("Email").GetSection("SmtpServer").Value;
                var smtpSsl = Convert.ToBoolean(Configuration.GetSection("Email").GetSection("EnableSsl").Value);
                var smtpSslType = Convert.ToInt32(Configuration.GetSection("Email").GetSection("EnableSslInt").Value);
                MailboxAddress from = new MailboxAddress(fromName, fromMail);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(fromName, toMail);
                message.To.Add(to);

                message.Subject = subject;
                //Design
                var builder = new BodyBuilder();

                string messageBodyHtml = bodyHTML;
                builder.HtmlBody = messageBodyHtml;
                builder.TextBody = bodyText;
                message.Body = builder.ToMessageBody();
                message.Body = builder.ToMessageBody();

                //STMP server
                SmtpClient client = new SmtpClient();
                if (smtpSslType == 0)
                {
                    SecureSocketOptions options = SecureSocketOptions.None;
                    client.Connect(smtpServer, Convert.ToInt32(smtpPort), options);
                }
                else
                {
                    client.Connect(smtpServer, Convert.ToInt32(smtpPort), smtpSsl);
                }
                client.Authenticate(fromMail, fromPassword);
                client.Send(message);
                return passwordChangeLink;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
