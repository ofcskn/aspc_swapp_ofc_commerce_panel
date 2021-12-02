using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admin.Models
{
    public class ResetPassword
    {
        public string NameSurname { get; set; }
        public string PasswordLink { get; set; }
    }
}
