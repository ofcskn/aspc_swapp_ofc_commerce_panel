using admin.Models;
using Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace admin.ViewModel
{
    public class EmailInboxViewModel
    {
        public string Filter { get; set; }
        public PaginatedList<Email> Emails { get; set; }
        public IQueryable<EmailStatus> EmailStatus { get; set; }
        public IQueryable<EmailAttachments> EmailAttachments { get; set; }
        public string SenderName { get; set; }
    }
}
