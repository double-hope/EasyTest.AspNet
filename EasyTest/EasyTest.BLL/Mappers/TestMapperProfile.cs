using AutoMapper;
using EasyTest.DAL.Entities;
using EasyTest.Shared.DTO.Test;

namespace EasyTest.BLL.Mappers
{
    public class TestMapperProfile : Profile
    {
        public TestMapperProfile()
        {
            CreateMap<TestCreateDto, Test>()
                .ForMember(dest => dest.Questions, opt => opt.Ignore())
                .ForMember(dest => dest.QuestionTests, opt => opt.Ignore());
            CreateMap<Test, TestDto>();
			CreateMap<TestEditDto, Test>()
				.ForMember(dest => dest.Questions, opt => opt.Ignore())
				.ForMember(dest => dest.QuestionTests, opt => opt.Ignore());

            CreateMap<Test, UserTestDto>().ReverseMap();
        }
    }
}
