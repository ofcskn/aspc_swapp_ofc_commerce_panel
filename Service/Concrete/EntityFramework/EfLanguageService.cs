using Entity.Context;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Service.Abstract;
using Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Concrete.EntityFramework
{
    public class EfLanguageService:EfGenericService<Language>,ILanguageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EfLanguageService(SwappDbContext _context, IHttpContextAccessor httpContextAccessor) : base(_context)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
        public string GetByShortLang(string shortLang)
        {
            if (_db.Language.Any(p => p.ShortTitle == shortLang))
            {
                return _db.Language.FirstOrDefault(p => p.ShortTitle == shortLang).Title;
            }
            return null;
        }
            
        public Language GetLanguageByShortLang(string shortLang)
        {
            if (_db.Language.Any(p => p.ShortTitle == shortLang))
            {
                return _db.Language.FirstOrDefault(p => p.ShortTitle == shortLang);
            }
            return null;
        }
    }
}
