
using Entity.Context;
using Entity.Models;
using Entity.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Service.Abstract;
using Service.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Concrete.EntityFramework
{
    public class EfEmailAttachmentService : EfGenericService<EmailAttachments>, IEmailAttachmentService
    {
        public IMediaService _mediaService;
        public EfEmailAttachmentService(SwappDbContext _context, IMediaService mediaService) : base(_context)
        {
            _mediaService = mediaService;
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public void AddEmailAttachments(Email entity, IEnumerable<IFormFile> attachments)
        {
            try
            {
                foreach (var attachment in attachments)
                {
                    if (attachment != null)
                    {
                        if (attachment.Length < 10485760)
                        {
                            var newFileName = _mediaService.InsertFile("/uploads/email/", attachment.FileName, "admin", attachment);

                            EmailAttachments ea = new EmailAttachments
                            {
                                Filename = newFileName,
                                SendDate = DateTime.Now,
                                ContentType = attachment.ContentType,
                                Size = attachment.Length.ToString(),
                                MailId = entity.Id
                            };

                            _db.EmailAttachments.Add(ea);

                        }
                        else
                        {

                        }
                    }
                }
                _db.SaveChanges();
            }
            catch (Exception)
            {
            }
        }    
    }
}
