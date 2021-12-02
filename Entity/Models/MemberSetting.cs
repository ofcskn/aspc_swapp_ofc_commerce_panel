using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class MemberSetting
    {
        public int Id { get; set; }
        public int? MemberId { get; set; }
        public string MemberRole { get; set; }
        public string SettingJson { get; set; }
    }
}
