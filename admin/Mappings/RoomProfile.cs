using AutoMapper;
using Entity.Models;
using admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admin.Mappings
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<MessageRooms, RoomViewModel>();
            CreateMap<RoomViewModel, MessageRooms>();
        }
    }
}
