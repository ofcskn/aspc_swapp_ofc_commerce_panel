using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class ViewMessageCurrent
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public string Mail { get; set; }
        public string RoomName { get; set; }
        public int? RoomId { get; set; }
        public string Message { get; set; }
        public DateTime SendDate { get; set; }
        public string SenderRole { get; set; }
    }
}
