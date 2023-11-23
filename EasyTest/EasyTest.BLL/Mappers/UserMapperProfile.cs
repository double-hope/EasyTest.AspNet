using AutoMapper;
using EasyTest.DAL.Entities;
using EasyTest.Shared.DTO.Test;
using EasyTest.Shared.DTO.User;

namespace EasyTest.BLL.Mappers
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserRegisterDto, User>()
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(u => u.Name));
        }
    }
}
