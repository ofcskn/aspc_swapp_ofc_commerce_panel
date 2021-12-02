using Entity.Models;
using Entity.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IEmailAttachmentService : IGenericService<EmailAttachments>
    {
        void AddEmailAttachments(Email entity, IEnumerable<IFormFile> attachments);

    }
}
