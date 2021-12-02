using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IBlogService : IGenericService<Blog>
    {
        string GenerateAndInsertPermalink(string title);
        Blog GetByPermalink(string permalink);
    }
}
