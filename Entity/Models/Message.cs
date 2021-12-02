using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Message
    {
        public int Id { get; set; }
        public string Message1 { get; set; }
        public DateTime SendDate { get; set; }
        public int? ReceiverId { get; set; }
        public string ReceiverRole { get; set; }
        public int SenderId { get; set; }
        public string SenderRole { get; set; }
        public int? RoomId { get; set; }
    }
}
