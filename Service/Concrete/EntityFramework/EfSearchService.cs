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

namespace Service.Concrete.EntityFramework
{
    public class EfSearchService : EfGenericService<Search>, ISearchService
    {
        public EfSearchService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public List<Search> GetAllByEnabled(bool b)
        {
            return _db.Search.Where(p => p.Enabled == b).ToList();
        }
        public List<Search> GetAllByFilter(string q)
        {
            List<Search> filtered = GetAllByEnabled(true);
            if (!string.IsNullOrEmpty(q) && filtered.Count > 0)
            {
                filtered = filtered.Where(p => p.Title.Contains(q, StringComparison.InvariantCultureIgnoreCase)
                || p.Description.Contains(q, StringComparison.InvariantCultureIgnoreCase)
                || p.Permalink.Contains(q, StringComparison.InvariantCultureIgnoreCase)).ToList();
                //|| p.Keywords.Contains(q, StringComparison.InvariantCultureIgnoreCase));
            }
            return filtered;
        }
    }
}
