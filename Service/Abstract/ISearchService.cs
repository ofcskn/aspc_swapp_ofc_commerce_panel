using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface ISearchService : IGenericService<Search>
    {
        List<Search> GetAllByEnabled(bool b);
        List<Search> GetAllByFilter(string q);
    }
}
