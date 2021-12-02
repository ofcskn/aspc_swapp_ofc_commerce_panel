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

namespace Service.Concrete.EntityFramework
{
    public class EfInvoiceService : EfGenericService<Invoice>, IInvoiceService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EfInvoiceService(SwappDbContext _context, IHttpContextAccessor httpContextAccessor) : base(_context)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public Invoice GetInvoice(int id)
        {
            return _db.Invoice.Include(p => p.InvoiceDetail).Include(p=>p.Current).Include(p => p.SaleProcess).Include(p => p.InvoiceDetail)
                .FirstOrDefault(p => p.Id == id) ;
        }

        public Invoice GetInvoiceById(int id)
        {
            return _db.Invoice.Include(p => p.InvoiceDetail).Include(p => p.Current).Include(p => p.SaleProcess).Include(p => p.Staff).FirstOrDefault(p=>p.Id == id);
        }
        public IQueryable<Invoice> GetAllWithDiagram()
        {
            return _db.Invoice.Include(p => p.Current).Include(p => p.SaleProcess).Include(p => p.Staff);
        }
        public string GetIdByStaff
        {
            get { return _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == "id").Value; }
        }
    }
}
