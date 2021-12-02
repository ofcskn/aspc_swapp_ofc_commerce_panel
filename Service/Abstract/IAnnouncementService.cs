using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface IAnnouncementService : IGenericService<Announcement>
    {
        IQueryable<Announcement> GetAllByEnabled(bool status);
         IQueryable<Announcement> GetAllByEnabledDate(bool status);
    }
}
