using Entity.Models;
using System;
using System.Collections.Generic;

namespace Entity.ViewModel
{
    public partial class EmailViewModel
    {
        public Invoice Invoice { get; set; }
        public string Type { get; set; }

        //For Newsletter
        public string SenderNameSurname { get; set; }
        public string ReceiverMail { get; set; }
        public string SenderMessage { get; set; }
        public string SenderLink { get; set; }
        public string SenderWelcomeText { get; set; }
        public DateTime SubscribeDate { get; set; }

        public string ConfirmLink { get; set; }
        public string UnsubscribeLink { get; set; }

        //For Normal Mail
        public string Subject { get; set; }
        public string BodyText { get; set; }
    }
}
