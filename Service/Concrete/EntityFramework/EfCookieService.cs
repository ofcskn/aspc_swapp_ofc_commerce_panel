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
    public class EfCookieService : ICookieService
    {
        private readonly SwappDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EfCookieService(SwappDbContext db, IHttpContextAccessor httpContextAccessor) : base()
        {
            _httpContextAccessor = httpContextAccessor;
            _db = db;
        }
        public string GetCookie(string key)
        {
            var langCookie = _httpContextAccessor.HttpContext.Request.Cookies[key];
            return langCookie;
        }
        public void SetCookie(string key, string value, CookieOptions option)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, option);
        }
        public void RemoveCookie(string key)
        {
            string value = "";
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(-1);
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, option);
        }
        public string GetWwwLangByCookie()
        {
            string lang;
            string key = "Language";

            var encryptedKey = Cipher.Encrypt(key, "language");
            var encryptedValue = GetCookie(encryptedKey);

            var decryptValue = Cipher.Decrypt(encryptedValue, "language");
            lang = decryptValue;
            lang = "tr";
            return lang;
        }
        public PageAnalysis GetPageAnalysisId()
        {
            string encrypted = Cipher.Encrypt("SayfaAnaliz", "PageAnalysis");
            var encyrptedAnalysisId = GetCookie(encrypted);
            string analysisId = Cipher.Decrypt(encyrptedAnalysisId, "PageAnalysis");
            return _db.PageAnalysis.FirstOrDefault(p => p.Id == Convert.ToInt32(analysisId));
        }
        public PageAnalysis GetPageAnalysisIdWWW()
        {
            string encrypted = Cipher.Encrypt("SayfaAnalizWWW", "PageAnalysisWWW");
            var encyrptedAnalysisId = GetCookie(encrypted);
            string analysisId = Cipher.Decrypt(encyrptedAnalysisId, "PageAnalysisWWW");
            return _db.PageAnalysis.FirstOrDefault(p => p.Id == Convert.ToInt32(analysisId));
        }
        public void AddPAWithCookie(PageAnalysis pa, string encrypted, CookieOptions option)
        {
            _db.PageAnalysis.Add(pa);
            _db.SaveChanges();
            string encryptedAnalysisId = Cipher.Encrypt(pa.Id.ToString(), "PageAnalysis");
            SetCookie(encrypted, encryptedAnalysisId, option);
        }
    }
}
