using AutoMapper;
using EasyTest.DAL.Entities;
using EasyTest.Shared.DTO.Session;

namespace EasyTest.BLL.Mappers
{
	public class SessionMapperProfile : Profile
	{
		public SessionMapperProfile()
		{
			CreateMap<SessionCreateDto, TestSession>()
				.ForMember(dest => dest.Status, opt => opt.Ignore())
				.ForMember(dest => dest.Questions, opt => opt.Ignore())
				.ForMember(dest => dest.SessionQuestions, opt => opt.Ignore())
				.ForMember(dest => dest.Answers, opt => opt.Ignore())
				.ForMember(dest => dest.SessionAnswers, opt => opt.Ignore());
			CreateMap<TestSession, SessionDto>();
		}

	}
}
