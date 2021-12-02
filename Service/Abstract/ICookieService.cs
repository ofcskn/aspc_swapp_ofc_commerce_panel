using Entity.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Abstract
{
    public interface ICookieService
    {
        string GetCookie(string key);
        void SetCookie(string key, string value, CookieOptions option);
        void RemoveCookie(string key);
        string GetWwwLangByCookie();
        PageAnalysis GetPageAnalysisId();
        void AddPAWithCookie(PageAnalysis pa, string encrypted, CookieOptions option);
        PageAnalysis GetPageAnalysisIdWWW();
    }
}
