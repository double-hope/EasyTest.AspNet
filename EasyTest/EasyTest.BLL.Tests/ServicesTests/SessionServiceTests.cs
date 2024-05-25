using AutoMapper;
using EasyTest.BLL.Mappers;
using EasyTest.BLL.Services;
using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using EasyTest.Shared.Constants;
using EasyTest.Shared.DTO.Question;
using EasyTest.Shared.DTO.Session;
using EasyTest.Shared.Enums;
using FakeItEasy;
using Mono.Reflection;

namespace EasyTest.BLL.Tests.Services
{
	[Trait("Category", "Unit")]
	public class SessionServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionServiceTests()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new SessionAnswerMapperProfile());
                mc.AddProfile(new SessionMapperProfile());
                mc.AddProfile(new AnswerMapperProfile());
            });

            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public async Task SessionService_CreateSession_ReturnsNewSession()
        {
            // Arrange
            var sessionService = new SessionService(_unitOfWork, _mapper);
            var sessionCreateDto = new SessionCreateDto { TestId = Guid.NewGuid() };
            var userEmail = string.Empty;

            A.CallTo(() => _unitOfWork.UserRepository.GetByEmail(userEmail)).Returns(A.Fake<User>());
            A.CallTo(() => _unitOfWork.TestSessionRepository.GetInProgressSession(A<Guid>.Ignored, sessionCreateDto.TestId)).Returns(Task.FromResult<TestSession>(null));

            // Act
            var result = await sessionService.Create(sessionCreateDto, userEmail);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Success));
            Assert.Equal("Session created successfully", result.Message);
        }

		[Fact]
		public async Task SessionService_CreateSession_ReturnsUserNotFound()
		{
			// Arrange
			var sessionService = new SessionService(_unitOfWork, _mapper);
			var sessionCreateDto = new SessionCreateDto { TestId = Guid.NewGuid() };
			var userEmail = string.Empty;

			A.CallTo(() => _unitOfWork.UserRepository.GetByEmail(userEmail)).Returns(Task.FromResult<User>(null));
			
			// Act
			var result = await sessionService.Create(sessionCreateDto, userEmail);

			// Assert
			Assert.True(result.Status.Equals(ResponseStatusCodesConst.Error));
			Assert.Equal("User not found", result.Message);
		}

		[Fact]
		public async Task SessionService_CreateSession_ReturnsTestNotFound()
		{
			// Arrange
			var sessionService = new SessionService(_unitOfWork, _mapper);
			var sessionCreateDto = new SessionCreateDto { TestId = Guid.NewGuid() };
			var userEmail = string.Empty;

			A.CallTo(() => _unitOfWork.UserRepository.GetByEmail(userEmail)).Returns(A.Fake<User>());
			A.CallTo(() => _unitOfWork.TestSessionRepository.GetInProgressSession(A<Guid>.Ignored, sessionCreateDto.TestId)).Returns(Task.FromResult<TestSession>(null));
			A.CallTo(() => _unitOfWork.TestRepository.GetById(A<Guid>.Ignored)).Returns(Task.FromResult<Test>(null));

			// Act
			var result = await sessionService.Create(sessionCreateDto, userEmail);

			// Assert
			Assert.True(result.Status.Equals(ResponseStatusCodesConst.Error));
			Assert.Equal("Test not found", result.Message);
		}

		[Fact]
        public async Task SessionService_CreateSession_ReturnsInProgressSession()
        {
            // Arrange
            var sessionService = new SessionService(_unitOfWork, _mapper);
			var sessionCreateDto = new SessionCreateDto { TestId = Guid.NewGuid() };
			var userEmail = string.Empty;

			A.CallTo(() => _unitOfWork.UserRepository.GetByEmail(userEmail)).Returns(A.Fake<User>());
			A.CallTo(() => _unitOfWork.TestSessionRepository.GetInProgressSession(A<Guid>.Ignored, sessionCreateDto.TestId)).Returns(A.Fake<TestSession>());

			// Act
			var result = await sessionService.Create(sessionCreateDto, userEmail);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Success));
            Assert.Equal("Return session created early", result.Message);
        }

        [Fact]
        public async Task SessionService_CreateSession_ReturnsNoMoreAttempts()
        {
            // Arrange
            var sessionService = new SessionService(_unitOfWork, _mapper);
			var sessionCreateDto = new SessionCreateDto { TestId = Guid.NewGuid() };
			var userEmail = string.Empty;

			A.CallTo(() => _unitOfWork.UserRepository.GetByEmail(userEmail)).Returns(A.Fake<User>());
			A.CallTo(() => _unitOfWork.TestSessionRepository.GetInProgressSession(A<Guid>.Ignored, sessionCreateDto.TestId)).Returns(Task.FromResult<TestSession>(null));
            A.CallTo(() => _unitOfWork.TestSessionRepository.GetAllUserSessions(A<Guid>.Ignored, sessionCreateDto.TestId))
                .Returns(new List<TestSession>() { A.Fake<TestSession>() });
            A.CallTo(() => _unitOfWork.TestRepository.GetById(sessionCreateDto.TestId))
                .Returns(Task.FromResult(A.Fake<Test>()));

            // Act
            var result = await sessionService.Create(sessionCreateDto, userEmail);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Error));
            Assert.Equal("You have used all your attempts", result.Message);
        }

        [Fact]
        public async Task SessionService_AnswerTheQuestion_WithIncorrectAnswer_ReturnsSuccessResponse()
        {
            // Arrange
            var sessionService = new SessionService(_unitOfWork, _mapper);
            var sessionId = Guid.NewGuid();
            var answerId = Guid.NewGuid();

            var fakeAnswer = A.Fake<Answer>();

            A.CallTo(() => _unitOfWork.AnswerRepository.GetById(answerId)).Returns(Task.FromResult(fakeAnswer));

            // Act
            var result = await sessionService.AnswerTheQuestion(sessionId, answerId);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Success));
            Assert.NotNull(result.Data);
            Assert.Equal("New question", result.Message);
            Assert.False(result.Data.IsCorrect);
        }

        [Fact]
        public async Task SessionService_IfGetResult_WithAllQuestionsAnswered_ReturnsTrue()
        {
            // Arrange
            var sessionService = new SessionService(_unitOfWork, _mapper);
            var sessionId = Guid.NewGuid();

            var fakeTestSession = A.Fake<TestSession>();

            A.CallTo(() => _unitOfWork.TestSessionRepository.GetSession(sessionId)).Returns(Task.FromResult(fakeTestSession));

            // Act
            var result = await sessionService.IfGetResult(sessionId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SessionService_IfGetResult_WithUnansweredQuestions_ReturnsFalse()
        {
            // Arrange
            var sessionService = new SessionService(_unitOfWork, _mapper);
            var sessionId = Guid.NewGuid();
            var testId = Guid.NewGuid();

            var fakeTestSession = A.Fake<TestSession>();
            fakeTestSession.TestId = testId;

            var fakeTest = A.Fake<Test>();
            fakeTest.Id = testId;
            fakeTest.QuestionsAttempted = 1;

            A.CallTo(() => _unitOfWork.TestSessionRepository.GetSession(sessionId)).Returns(Task.FromResult(fakeTestSession));
            A.CallTo(() => _unitOfWork.TestRepository.GetById(testId)).Returns(Task.FromResult(fakeTest));

            // Act
            var result = await sessionService.IfGetResult(sessionId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SessionService_GetResult_ReturnsSessionResult()
        {
            // Arrange
            var sessionService = new SessionService(_unitOfWork, _mapper);
            var sessionId = Guid.NewGuid();
            var testId = Guid.NewGuid();

            var fakeTestSession = A.Fake<TestSession>();
            fakeTestSession.Status = TestStatus.InProgress;

            var fakeSessionQuestion1 = A.Fake<SessionQuestion>();
            fakeSessionQuestion1.QuestionId = Guid.NewGuid();

            var fakeSessionQuestion2 = A.Fake<SessionQuestion>();
            fakeSessionQuestion2.QuestionId = Guid.NewGuid();

            A.CallTo(() => _unitOfWork.TestSessionRepository.GetSession(sessionId))
             .ReturnsLazily(async () =>
             {
                 var fakeTestSession = A.Fake<TestSession>();

                 var propInfo = typeof(TestSession).GetProperty("SessionQuestions");
                 var fieldInfo = propInfo.GetBackingField();
                 fieldInfo.SetValue(fakeTestSession, new List<SessionQuestion> { fakeSessionQuestion1, fakeSessionQuestion2 });

                 return fakeTestSession;
             });
            A.CallTo(() => _unitOfWork.SessionAnswerRepository.GetCorrectAnswers(sessionId)).Returns(Task.FromResult(new List<SessionAnswer> { A.Fake<SessionAnswer>() }));

            // Act
            var result = await sessionService.GetResult(sessionId);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Success));
        }

        [Fact]
        public async Task SessionService_NextQuestion_WithAvailableQuestions_ReturnsQuestionDto()
        {
            // Arrange
            var sessionService = new SessionService(_unitOfWork, _mapper);
            var sessionId = Guid.NewGuid();
            var testId = Guid.NewGuid();

            var fakeTestSession = A.Fake<TestSession>();
            fakeTestSession.TestId = testId;
            var fakeTest = A.Fake<Test>();
            fakeTest.Id = testId;
            fakeTest.QuestionsAttempted = 1;
            var fakeQuestion = A.Fake<Question>();
            var fakeAnswer = A.Fake<Answer>();

            A.CallTo(() => _unitOfWork.TestSessionRepository.GetSession(sessionId)).Returns(Task.FromResult(fakeTestSession));
            A.CallTo(() => _unitOfWork.QuestionTestRepository.GetQuestionsByTestId(fakeTestSession.TestId)).Returns(Task.FromResult(new List<Question> { fakeQuestion }));
            A.CallTo(() => _unitOfWork.SessionQuestionRepository.Add(A<SessionQuestion>._)).Returns(Task.CompletedTask);
            A.CallTo(() => _unitOfWork.Save()).Returns(Task.CompletedTask);
            A.CallTo(() => _unitOfWork.AnswerRepository.GetByQuestionId(fakeQuestion.Id)).Returns(Task.FromResult(new List<Answer> { fakeAnswer }));
            A.CallTo(() => _unitOfWork.TestRepository.GetById(testId)).Returns(Task.FromResult(fakeTest));

            // Act
            var result = await sessionService.NextQuestion(sessionId);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Success));
            Assert.IsType<QuestionNextDto>(result.Data);
        }

        [Fact]
        public async Task SessionService_NextQuestion_WithAvailableQuestions_ReturnsNoAvailableQuestions()
        {
            // Arrange
            var sessionService = new SessionService(_unitOfWork, _mapper);
            var sessionId = Guid.NewGuid();
            var testId = Guid.NewGuid();

            var fakeTestSession = A.Fake<TestSession>();

            A.CallTo(() => _unitOfWork.TestSessionRepository.GetSession(sessionId)).Returns(Task.FromResult(fakeTestSession));
            A.CallTo(() => _unitOfWork.QuestionTestRepository.GetQuestionsByTestId(fakeTestSession.TestId)).Returns(Task.FromResult(new List<Question>()));

            // Act
            var result = await sessionService.NextQuestion(sessionId);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Error));
            Assert.Equal("No more available questions for this test", result.Message);
        }

        [Fact]
        public async Task SessionService_NextQuestion_WithAvailableQuestions_ReturnsNoMoreQuestions()
        {
            // Arrange
            var sessionService = new SessionService(_unitOfWork, _mapper);
            var sessionId = Guid.NewGuid();

            var fakeTestSession = A.Fake<TestSession>();
            var fakeQuestion = A.Fake<Question>();

            A.CallTo(() => _unitOfWork.TestSessionRepository.GetSession(sessionId)).Returns(Task.FromResult(fakeTestSession));
            A.CallTo(() => _unitOfWork.QuestionTestRepository.GetQuestionsByTestId(fakeTestSession.TestId)).Returns(Task.FromResult(new List<Question> { fakeQuestion }));

            // Act
            var result = await sessionService.NextQuestion(sessionId);

            // Assert
            Assert.True(result.Status.Equals(ResponseStatusCodesConst.Error));
            Assert.Equal("No more questions for this test", result.Message);
        }
    }
}
