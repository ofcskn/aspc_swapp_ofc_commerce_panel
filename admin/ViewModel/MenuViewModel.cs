using Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace admin.ViewModel
{
    public class MenuViewModel
    {
        public IQueryable<Menu> Menus { get; set; }
        public Menu Menu { get; set; }
        public string userRole { get; set; }
        public string userImage { get; set; }
        public string userName { get; set; }
    }
}
