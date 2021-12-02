using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Setting
    {
        public int Id { get; set; }
        public string SettingJsonData { get; set; }
        public int WebsiteId { get; set; }
        public string Lang { get; set; }
    }
}
