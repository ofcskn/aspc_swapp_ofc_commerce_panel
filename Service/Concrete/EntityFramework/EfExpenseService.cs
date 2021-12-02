
using Entity.Context;
using Entity.Models;
using Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Concrete.EntityFramework
{
    public class EfExpenseService :  IExpenseService
    {
        public EfExpenseService(SwappDbContext _context) : base()
        {
        }
    }
}
