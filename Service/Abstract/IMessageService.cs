using Entity.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Abstract
{
    public interface IMessageService:IGenericService<Message>
    {
    }
}
