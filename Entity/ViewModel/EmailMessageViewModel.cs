using System;
using System.Collections.Generic;

namespace Entity.ViewModels
{
    public partial class EmailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public int SenderName { get; set; }
        public string SenderRole { get; set; }
        public int ReceiverName { get; set; }
        public string ReceiverRole { get; set; }
        public DateTime SendDate { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? ReadDate { get; set; }

    }
}
