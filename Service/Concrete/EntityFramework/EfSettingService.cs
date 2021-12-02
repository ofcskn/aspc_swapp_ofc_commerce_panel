using Entity.Context;
using Entity.Models;
using Service.Abstract;
using Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Concrete.EntityFramework
{
    public class EfSettingService : EfGenericService<Setting>, ISettingService
    {
        public EfSettingService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public Setting GetByLang(string lang)
        {
            return _db.Setting.FirstOrDefault(p => p.Lang == lang);
        }
    }
}
