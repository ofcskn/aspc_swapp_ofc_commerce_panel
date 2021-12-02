using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class MessageUserRooms
    {
        public int Id { get; set; }
        public int? RoomId { get; set; }
        public int? UserId { get; set; }
    }
}
