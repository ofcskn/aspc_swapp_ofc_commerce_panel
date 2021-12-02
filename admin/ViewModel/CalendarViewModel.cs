using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace admin.ViewModel
{
    public class CalendarViewModel
    {
        public Calendar Calendar { get; set; }
        public List<Calendar> Calendars { get; set; }
    }
}
