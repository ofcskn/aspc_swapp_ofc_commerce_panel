using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class NewsletterMail
    {
        public int Id { get; set; }
        public int NewsletterId { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime? ReadDate { get; set; }
    }
}
