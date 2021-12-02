using AutoMapper;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using admin.ViewModel;

namespace admin.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Staff, UserViewModel>()
               .ForMember(dst => dst.Avatar, opt => opt.MapFrom(x => x.Image))
               .ForMember(dst => dst.FullName, opt => opt.MapFrom(x => x.Name + " " + x.Surname))
                .ForMember(dst => dst.Username, opt => opt.MapFrom(x => x.UserName));
            // .ForMember(dst => dst.AvatarSrc, opt => opt.MapFrom(x => "staff"));
            CreateMap<UserViewModel, Staff>();
        }
    }
}
