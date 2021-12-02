using admin.ViewModels;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admin.ViewModel
{
    public class NavbarViewModel
    {
        public List<EmailMessageViewModel> Emails { get; set; }

        public int NotReadEmailsCount { get; set; }
        public int NotificationCount { get; set; }
        public int NotReadMessageCount { get; set; }
        public int NotReadCargoCompanyCount { get; set; }
    }
}
