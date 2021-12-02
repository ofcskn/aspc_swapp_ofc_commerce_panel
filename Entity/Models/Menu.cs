using System;
using System.Collections.Generic;

namespace Entity.Models
{
    public partial class Menu
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Priority { get; set; }
        public bool Enabled { get; set; }
        public int? ParentId { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string FontAwesomeClass { get; set; }
        public string Lang { get; set; }
        public string RouteValue { get; set; }
        public string RouteString { get; set; }
        public string Role { get; set; }
    }
}
