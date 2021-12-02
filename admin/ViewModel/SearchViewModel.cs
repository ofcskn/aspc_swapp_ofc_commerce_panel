using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admin.ViewModel
{
    public class SearchViewModel
    {
        public List<Search> SearchResults { get; set; }
        public List<Product> Products { get; set; }
    }
}
