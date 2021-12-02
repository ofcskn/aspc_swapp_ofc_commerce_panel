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

namespace Service.Concrete.EntityFramework
{
    public class EfMessageService : EfGenericService<Message>, IMessageService
    {
        public EfMessageService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
    }
}
