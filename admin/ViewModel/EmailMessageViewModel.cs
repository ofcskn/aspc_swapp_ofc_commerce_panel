using System;
using System.Collections.Generic;

namespace admin.ViewModels
{
    public partial class EmailMessageViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string SenderName { get; set; }
        public string SenderRole { get; set; }
        public DateTime SendDate { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? ReadDate { get; set; }

    }
}
