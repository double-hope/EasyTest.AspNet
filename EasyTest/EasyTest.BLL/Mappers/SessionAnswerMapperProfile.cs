using AutoMapper;
using EasyTest.DAL.Entities;
using EasyTest.Shared.DTO.Answer;

namespace EasyTest.BLL.Mappers
{
	public class SessionAnswerMapperProfile : Profile
	{
		public SessionAnswerMapperProfile()
		{
			CreateMap<SessionAnswer, SessionAnswerDto>();
		}

	}
}
