using AutoMapper;
using EasyTest.DAL.Entities;
using EasyTest.Shared.DTO.Answer;

namespace EasyTest.BLL.Mappers
{
    public class AnswerMapperProfile : Profile
    {
        public AnswerMapperProfile()
        {
            CreateMap<AnswerDto, Answer>();
            CreateMap<Answer, AnswerDto>();
        }
    }
}
