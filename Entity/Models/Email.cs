using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Email
    {
        public Email()
        {
            EmailAttachments = new HashSet<EmailAttachments>();
            EmailStatus = new HashSet<EmailStatus>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public int SenderId { get; set; }
        public string SenderRole { get; set; }
        public int? ReceiverId { get; set; }
        public string ReceiverRole { get; set; }
        public DateTime? SendDate { get; set; }
        public bool? DraftEnabled { get; set; }
        public DateTime? DraftDate { get; set; }
        public string SenderName { get; set; }

        public virtual ICollection<EmailAttachments> EmailAttachments { get; set; }
        public virtual ICollection<EmailStatus> EmailStatus { get; set; }
    }
}
