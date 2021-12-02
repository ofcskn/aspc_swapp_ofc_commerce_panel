using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IInvoiceService : IGenericService<Invoice>
    {
        string GetIdByStaff { get; }
        Invoice GetInvoice(int id);
        IQueryable<Invoice> GetAllWithDiagram();
        Invoice GetInvoiceById(int id);
    }
}
