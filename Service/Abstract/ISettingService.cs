using Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Abstract
{
    public interface ISettingService:IGenericService<Setting>
    {
        public Setting GetByLang(string lang);
    }
}
