using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admin.ViewModel
{
    public class SettingViewModel
    {
        public Admin Admin { get; set; }
        public Current Current { get; set; }
        public Staff Staff { get; set; }

    }
}
