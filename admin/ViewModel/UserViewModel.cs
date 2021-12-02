using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admin.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string AvatarSrc { get; set; }
        public string CurrentRoom { get; set; }
        public string Device { get; set; }
        public string Role { get; set; }
        public bool Enabled { get; set; }
    }
}
