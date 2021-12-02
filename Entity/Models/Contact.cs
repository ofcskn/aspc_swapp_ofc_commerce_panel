using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string Ip { get; set; }
        public bool Report { get; set; }
        public string Subject { get; set; }
    }
}
