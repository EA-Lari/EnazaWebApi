using AutoMapper;
using EnazaWebApi.Data.Models;
using EnazaWebApi.Logic.Dto;

namespace EnazaWebApi.Application
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserShowDto>()
                .ForMember(x => x.Group, opt => opt.MapFrom(y => y.Group))
                .ForMember(x => x.State, opt =>opt.MapFrom(y => y.State));
            CreateMap<UserEditDto, User>()
                .ForMember(x => x.Group, opt => opt.Ignore())
                .ForMember(x => x.State, opt => opt.Ignore());
            CreateMap<UserGroup, UserGroupShowDto>();
            CreateMap<UserState, UserStateShowDto>();
        }
    }
}
