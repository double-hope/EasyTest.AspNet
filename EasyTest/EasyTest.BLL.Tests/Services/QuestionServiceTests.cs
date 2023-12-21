using AutoMapper;
using EasyTest.BLL.Interfaces;
using EasyTest.BLL.Mappers;
using EasyTest.BLL.Services;
using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Answer;
using EasyTest.Shared.DTO.Question;
using EasyTest.Shared.DTO.Response;
using FakeItEasy;
using Microsoft.EntityFrameworkCore.Storage;

namespace EasyTest.BLL.Tests.Services
{
    public class QuestionServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAnswerService _answerService;

        public QuestionServiceTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new QuestionMapperProfile());
            });

            _mapper = mappingConfig.CreateMapper();
            _answerService = A.Fake<IAnswerService>();
        }

        [Fact]
        public async Task CreateMany_SuccessfullyCreatesQuestions()
        {
            // Arrange
            var questionService = new QuestionService(_unitOfWork, _mapper, _answerService);
            var questionsDto = A.Fake<List<QuestionDto>>();
            var testId = Guid.NewGuid();

            A.CallTo(() => _unitOfWork.BeginTransaction()).Returns(A.Fake<IDbContextTransaction>());
            A.CallTo(() => _unitOfWork.Commit()).Returns(Task.CompletedTask);

            var createResponses = new List<Response<QuestionResponseDto>>
            {
                Response<QuestionResponseDto>.Success(A.Fake<QuestionResponseDto>()),
                Response<QuestionResponseDto>.Success(A.Fake <QuestionResponseDto>()),
            };

            // Act
            var result = await questionService.CreateMany(questionsDto, testId);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Success));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task CreateMany_SuccessfullyCreatesRealQuestions(List<QuestionDto> questionsDto)
        {
            // Arrange
            var questionService = new QuestionService(_unitOfWork, _mapper, _answerService);
            var testId = Guid.NewGuid();

            A.CallTo(() => _unitOfWork.BeginTransaction()).Returns(A.Fake<IDbContextTransaction>());
            A.CallTo(() => _unitOfWork.Rollback()).Returns(Task.CompletedTask);

            var createResponses = new List<Response<QuestionResponseDto>>
            {
                Response<QuestionResponseDto>.Error("Error creating question"),
                Response<QuestionResponseDto>.Success(A.Fake<QuestionResponseDto>()),
            };

            // Act
            var result = await questionService.CreateMany(questionsDto, testId);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Success));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task CreateMany_FailsOnCreateQuestionError(List<QuestionDto> questionsDto)
        {
            // Arrange
            var questionService = new QuestionService(_unitOfWork, _mapper, _answerService);
            var testId = Guid.NewGuid();

            A.CallTo(() => _unitOfWork.BeginTransaction()).Returns(A.Fake<IDbContextTransaction>());
            A.CallTo(() => _unitOfWork.Rollback()).Returns(Task.CompletedTask);
            A.CallTo(() => _unitOfWork.TestRepository.GetById(testId)).Returns(Task.FromResult<Test>(null));

            var createResponses = new List<Response<QuestionResponseDto>>
            {
                Response<QuestionResponseDto>.Error("Error creating question"),
                Response<QuestionResponseDto>.Success(A.Fake<QuestionResponseDto>()),
            };

            // Act
            var result = await questionService.CreateMany(questionsDto, testId);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Error));
        }

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] {
                new List<QuestionDto>
                {
                    new QuestionDto { Title = "Title#1", Text = "Text#1" },
                    new QuestionDto { Title = "Title#1", Text = "Text#2" },
                    new QuestionDto { Title = "Title#1", Text = "Text#3" }
                }
            }
        };

        [Fact]
        public async Task CreateMany_ThrowsExceptionOnCreate()
        {
            // Arrange
            var questionService = new QuestionService(_unitOfWork, _mapper, _answerService);
            var questionsDto = A.Fake<List<QuestionDto>>();
            var testId = Guid.NewGuid();

            A.CallTo(() => _unitOfWork.BeginTransaction()).Throws(new Exception("Cannot start transaction"));
            A.CallTo(() => _unitOfWork.Commit()).Returns(Task.CompletedTask);

            // Act
            var result = await questionService.CreateMany(questionsDto, testId);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Error));
            Assert.Equal("An error occurred", result.Message);
            Assert.Contains("Cannot start transaction", result.Errors);
        }


        [Fact]
        public async Task Create_SuccessfullyCreatesQuestion()
        {
            // Arrange
            var questionService = new QuestionService(_unitOfWork, _mapper, _answerService);
            var questionDto = A.Fake<QuestionDto>();
            var testId = Guid.NewGuid();

            A.CallTo(() => _unitOfWork.QuestionRepository.Add(A<Question>.Ignored)).Returns(Task.CompletedTask);
            A.CallTo(() => _unitOfWork.TestRepository.GetById(A<Guid>.Ignored)).Returns(A.Fake<Test>());
            A.CallTo(() => _unitOfWork.QuestionTestRepository.Add(A<QuestionTest>.Ignored)).Returns(Task.CompletedTask);
            A.CallTo(() => _answerService.CreateRange(A<List<AnswerDto>>.Ignored, A<Guid>.Ignored)).Returns(Response<IEnumerable<AnswerDto>>.Success(A.Fake<IEnumerable<AnswerDto>>(), "Answer created successful"));

            // Act
            var result = await questionService.Create(questionDto, testId);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Success));
        }

        [Fact]
        public async Task Create_FailsWhenTestNotFound()
        {
            // Arrange
            var questionService = new QuestionService(_unitOfWork, _mapper, _answerService);
            var questionDto = A.Fake<QuestionDto>();
            var testId = Guid.NewGuid();

            A.CallTo(() => _unitOfWork.QuestionRepository.Add(A<Question>.Ignored)).Returns(Task.CompletedTask);
            A.CallTo(() => _unitOfWork.TestRepository.GetById(A<Guid>.Ignored)).Returns((Test)null);

            // Act
            var result = await questionService.Create(questionDto, testId);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Error));
        }

        [Fact]
        public async Task Create_FailsWhenAnswerServiceFails()
        {
            // Arrange
            var questionService = new QuestionService(_unitOfWork, _mapper, _answerService);
            var questionDto = A.Fake<QuestionDto>();
            var testId = Guid.NewGuid();

            A.CallTo(() => _unitOfWork.QuestionRepository.Add(A<Question>.Ignored)).Returns(Task.CompletedTask);
            A.CallTo(() => _unitOfWork.TestRepository.GetById(A<Guid>.Ignored)).Returns(A.Fake<Test>());
            A.CallTo(() => _unitOfWork.QuestionTestRepository.Add(A<QuestionTest>.Ignored)).Returns(Task.CompletedTask);
            A.CallTo(() => _answerService.CreateRange(A<List<AnswerDto>>.Ignored, A<Guid>.Ignored)).Returns(Response<IEnumerable<AnswerDto>>.Error("Answer service error"));

            // Act
            var result = await questionService.Create(questionDto, testId);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Error));
        }
    }
}
