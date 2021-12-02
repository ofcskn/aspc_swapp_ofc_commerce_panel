using Entity.Context;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Service.Utilities;
using System.IO;
using MimeKit.Utils;
using MailKit.Security;

namespace Service.Concrete.EntityFramework
{
    public class EfCommonService : ICommonService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EfCommonService(SwappDbContext _context, IHttpContextAccessor httpContextAccessor) : base()
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string SendMailToDefault(string subject, string bodyHTML, string bodyText, IConfiguration Configuration)
        {
            MimeMessage message = new MimeMessage();
            string fromName = Configuration.GetSection("Email").GetSection("DisplayName").Value;
            string fromMail = Configuration.GetSection("Email").GetSection("From").Value;
            string toMail = Configuration.GetSection("Email").GetSection("To").Value;
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
            // Now we just need to set the message body and we're done
            message.Body = builder.ToMessageBody();

            //var image = builder.LinkedResources.Add(@"C:\Users\Joey\Documents\Selfies\selfie.jpg");

            // We may also want to attach a calendar event for Monica's party...
            //builder.Attachments.Add(@"C:\Users\Joey\Documents\party.ics");

            // Now we just need to set the message body and we're done
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
            return "success";
        }
        public string SendMailMultipleWithMail(string subject, string bodyHTML, string bodyText, IConfiguration Configuration, List<string> toMails, IEnumerable<IFormFile> Files)
        {
            MimeMessage message = new MimeMessage();
            string fromName = Configuration.GetSection("Email").GetSection("DisplayName").Value;
            string SenderName = Configuration.GetSection("Email").GetSection("SenderName").Value;
            string fromMail = Configuration.GetSection("Email").GetSection("From").Value;
            string fromPassword = Configuration.GetSection("Email").GetSection("Password").Value;
            string smtpPort = Configuration.GetSection("Email").GetSection("SmtpPort").Value;
            string smtpServer = Configuration.GetSection("Email").GetSection("SmtpServer").Value;
            var smtpSsl = Convert.ToBoolean(Configuration.GetSection("Email").GetSection("EnableSsl").Value);
            var smtpSslType = Convert.ToInt32(Configuration.GetSection("Email").GetSection("EnableSslInt").Value);

            if (bodyHTML == null)
            {
                bodyHTML = "<h2>Swapp-OFc</h2><p>" + bodyText + "</p><b>" + fromName + "</b><i>" + SenderName + "</i>";
            }
            foreach (var item in toMails)
            {
                MailboxAddress from = new MailboxAddress(fromName, fromMail);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(fromName, item);
                message.To.Add(to);

                message.Subject = subject;
                //Design
                var builder = new BodyBuilder();

                string messageBodyHtml = bodyHTML;
                builder.HtmlBody = messageBodyHtml;
                builder.TextBody = bodyText;
                // Now we just need to set the message body and we're done
                message.Body = builder.ToMessageBody();

                if (Files != null)
                {
                    foreach (var attachment in Files.ToList())
                    {
                        //var image = builder.LinkedResources.Add(@"C:\Users\Joey\Documents\Selfies\selfie.jpg");
                        //image.ContentId = MimeUtils.GenerateMessageId();
                        builder.Attachments.Add(attachment.FileName, attachment.OpenReadStream(), ContentType.Parse(attachment.ContentType));
                    }
                    builder.HtmlBody = string.Format(bodyHTML);
                }

                // Now we just need to set the message body and we're done
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
            }
            return "success";
        }
        public string SendMailToAnother(string subject, string bodyHTML, string bodyText, IConfiguration Configuration, string toEmail)
        {
            MimeMessage message = new MimeMessage();
            string fromName = Configuration.GetSection("Email").GetSection("DisplayName").Value;
            string SenderName = Configuration.GetSection("Email").GetSection("SenderName").Value;
            string fromMail = Configuration.GetSection("Email").GetSection("From").Value;
            string fromPassword = Configuration.GetSection("Email").GetSection("Password").Value;
            string smtpPort = Configuration.GetSection("Email").GetSection("SmtpPort").Value;
            string smtpServer = Configuration.GetSection("Email").GetSection("SmtpServer").Value;
            var smtpSsl = Convert.ToBoolean(Configuration.GetSection("Email").GetSection("EnableSsl").Value);
            var smtpSslType = Convert.ToInt32(Configuration.GetSection("Email").GetSection("EnableSslInt").Value);

            if (bodyHTML == null)
            {
                bodyHTML = "<h2>Swapp-ÖFC</h2><p>" + bodyText + "</p><b>" + fromName + "</b><i>" + SenderName + "</i>";
            }
            MailboxAddress from = new MailboxAddress(fromName, fromMail);
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress(fromName, toEmail);
            message.To.Add(to);

            message.Subject = subject;
            //Design
            var builder = new BodyBuilder();

            string messageBodyHtml = bodyHTML;
            builder.HtmlBody = messageBodyHtml;
            builder.TextBody = bodyText;
            // Now we just need to set the message body and we're done
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
            return "success";
        }
    }
}
