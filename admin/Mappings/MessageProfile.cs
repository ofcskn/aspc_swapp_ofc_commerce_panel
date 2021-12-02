using AutoMapper;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.Abstract;
using Service.Utilities;
using Entity.Context;
using admin.ViewModel;

namespace admin.Mappings
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<ViewMessageAll, MessageViewModel>()
                 .ForMember(dst => dst.From, opt => opt.MapFrom(x => x.Name + x.Surname))
                // .ForMember(dst => dst.To, opt => opt.MapFrom(x => ))
                .ForMember(dst => dst.Avatar, opt => opt.MapFrom(x => x.Image))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(x => BasicEmojis.ParseEmojis(x.Message)))
                .ForMember(dst => dst.AvatarSrc, opt => opt.MapFrom(x => x.SenderRole))
                .ForMember(dst => dst.Timestamp, opt => opt.MapFrom(x => x.SendDate.ToString("g")));
            CreateMap<MessageViewModel, ViewMessageAll>();
        }

    }
}
