using Entity.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Abstract
{
    public interface ILanguageService : IGenericService<Language>
    {
        string GetByShortLang(string shortLang);
        Language GetLanguageByShortLang(string shortLang);
    }
}
