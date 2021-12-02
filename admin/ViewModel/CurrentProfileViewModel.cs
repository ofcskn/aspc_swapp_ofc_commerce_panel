using Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace admin.ViewModel
{
    public class CurrentProfileViewModel
    {
        public Current Current { get; set; }
        public IQueryable<TimeLine> Timelines { get; set; }
        public List<AnnouncementAdminViewModel> Announcements { get; set; }
        public int NotificationCount { get; set; }
        public int EmailCount { get; set; }
        public int Score { get; set; }
    }
}
