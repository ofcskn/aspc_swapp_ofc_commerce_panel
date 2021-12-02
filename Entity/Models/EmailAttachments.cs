using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class EmailAttachments
    {
        public int Id { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public string Size { get; set; }
        public DateTime SendDate { get; set; }
        public int MailId { get; set; }

        public virtual Email Mail { get; set; }
    }
}
