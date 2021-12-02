using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace admin.ViewModel
{
    public class EmailSideBarViewModel
    {
        public int TotalReceived { get; set; }
        public int TotalSended { get; set; }
        public int TotalDrafted { get; set; }
        public int TotalFavourite { get; set; }
        public int TotalTrash { get; set; }
    }
}
