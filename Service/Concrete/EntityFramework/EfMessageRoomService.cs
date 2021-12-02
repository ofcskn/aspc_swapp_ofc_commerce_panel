
using Entity.Context;
using Entity.Models;
using Service.Abstract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Concrete.EntityFramework
{
    public class EfMessageRoomService : EfGenericService<MessageRooms>, IMessageRoomService
    {
        public EfMessageRoomService(SwappDbContext _context) : base(_context)
        {
        }
        public SwappDbContext _db
        {
            get { return _context as SwappDbContext; }
        }
    }
}
