using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Abstract
{
    public interface INotificationTypeService : IGenericService<NotificationType>
    {
        NotificationType GetNTByType(string type);
    }
}
