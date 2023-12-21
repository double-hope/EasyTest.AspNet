using AutoMapper;
using EasyTest.BLL.Mappers;
using EasyTest.BLL.Services;
using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Answer;
using FakeItEasy;

namespace EasyTest.BLL.Tests.Services
{
    public class AnswerServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AnswerServiceTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AnswerMapperProfile());
            });

            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public async Task AnswerService_CreateRange_ReturnSuccessWithEmptyAnswers()
        {
            var questionId = Guid.NewGuid();
            var answerDtos = A.Fake<List<AnswerDto>>();
            var service = new AnswerService(_unitOfWork, _mapper);

            var result = await service.CreateRange(answerDtos, questionId);

            Assert.NotNull(result);
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Success));
            Assert.Equal(answerDtos.Count, result.Data.Count());
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task AnswerService_CreateRange_ReturnSuccessWithAnswers(List<AnswerDto> answerDtos)
        {
            var questionId = Guid.NewGuid();
            var expectedAnswers = _mapper.Map<IEnumerable<Answer>>(answerDtos)
                .Select(answer =>
                {
                    answer.QuestionId = questionId;
                    return answer;
                });
            var service = new AnswerService(_unitOfWork, _mapper);

            var result = await service.CreateRange(answerDtos, questionId);

            Assert.NotNull(result);
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Success));
            Assert.Equal(answerDtos.Count, result.Data.Count());
        }


        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] {
                new List<AnswerDto>
                {
                    new AnswerDto { Id = Guid.NewGuid(), Text = "Answer#1", IsCorrect = false },
                    new AnswerDto { Id = Guid.NewGuid(), Text = "Answer#2", IsCorrect = true },
                    new AnswerDto { Id = Guid.NewGuid(), Text = "Answer#3", IsCorrect = false }
                }
            }
        };
    }
}
