using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class SpecificSetting
    {
        public int Id { get; set; }
        public string JsonData { get; set; }
        public string Language { get; set; }
        public int MemberId { get; set; }
        public string MemberRole { get; set; }
    }
}
