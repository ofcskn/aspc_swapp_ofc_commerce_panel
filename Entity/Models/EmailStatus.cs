using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class EmailStatus
    {
        public int Id { get; set; }
        public bool ReadStatus { get; set; }
        public DateTime? ReadDate { get; set; }
        public bool TrashStatus { get; set; }
        public DateTime? TrashDate { get; set; }
        public bool JunkStatus { get; set; }
        public DateTime? JunkDate { get; set; }
        public bool FavouriteStatus { get; set; }
        public DateTime? FavouriteDate { get; set; }
        public int EmailId { get; set; }
        public int UserId { get; set; }
        public string UserRole { get; set; }
        public bool PermanentStatus { get; set; }
        public DateTime? PermanentDate { get; set; }

        public virtual Email Email { get; set; }
    }
}
