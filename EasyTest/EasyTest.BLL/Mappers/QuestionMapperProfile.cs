using AutoMapper;
using EasyTest.DAL.Entities;
using EasyTest.Shared.DTO.Question;

namespace EasyTest.BLL.Mappers
{
    public class QuestionMapperProfile : Profile
    {
        public QuestionMapperProfile()
        {
            CreateMap<QuestionDto, Question>();
            CreateMap<Question, QuestionDto>();
            CreateMap<Question, QuestionResponseDto>();
		}
    }
}
