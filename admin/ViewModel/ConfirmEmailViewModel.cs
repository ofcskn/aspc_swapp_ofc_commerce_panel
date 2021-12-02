using admin.Models;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admin.ViewModel
{
    public class ConfirmEmailViewModel
    {
        public string SenderNameSurname { get; set; }
        public string ReceiverMail { get; set; }
        public string SenderMessage { get; set; }
        public string SenderLink { get; set; }
        public string SenderWelcomeText { get; set; }
        public DateTime SubscribeDate { get; set; }

        public string ConfirmLink { get; set; }
        public string UnsubscribeLink { get; set; }

        public string CurrentText { get; set; }
        public string CurrentUserName { get; set; }
        public string CurrentPassword { get; set; }

        public string StaffText { get; set; }
        public string StaffUserName { get; set; }
        public string StaffPassword { get; set; }

        public string UserText { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }

        public string ConfirmationCode { get; set; }
    }
}
