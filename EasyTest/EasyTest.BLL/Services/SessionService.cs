using AutoMapper;
using EasyTest.BLL.Interfaces;
using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
using EasyTest.Shared.DTO.Answer;
using EasyTest.Shared.DTO.Question;
using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Session;
using EasyTest.Shared.Enums;

namespace EasyTest.BLL.Services
{
	public class SessionService : Service, ISessionService
	{
		public SessionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

		public async Task<Response<SessionDto>> Create(SessionCreateDto sessionDto)
		{
			var user = (await _unitOfWork.UserRepository.GetByEmail(sessionDto.UserEmail));

			if(user == null)
			{
				return Response<SessionDto>.Error("User not found");
			}

			var userId = user.Id;

			var inProgressSession = await _unitOfWork.TestSessionRepository.GetInProgressSession(userId, sessionDto.TestId);

			if (inProgressSession != null)
			{
				return Response<SessionDto>.Success(_mapper.Map<SessionDto>(inProgressSession), "Return session created early");
			}

			var userSessions = await _unitOfWork.TestSessionRepository.GetAllUserSessions(userId, sessionDto.TestId);
			var test = await _unitOfWork.TestRepository.GetById(sessionDto.TestId);

			if (test == null)
			{
				return Response<SessionDto>.Error("Test not found");
			}

			if (userSessions.Count >= test.NumberOfAttempts)
			{
				return Response<SessionDto>.Error("You have used all your attempts");
			}

			var sessionE = _mapper.Map<TestSession>(sessionDto);

			sessionE.Status = TestStatus.InProgress;
			sessionE.UserId = userId;
			await _unitOfWork.TestSessionRepository.Add(sessionE);
			await _unitOfWork.Save();

			return Response<SessionDto>.Success(_mapper.Map<SessionDto>(sessionE), "Session created successfully");
		}

		public async Task<Response<QuestionNextDto>> NextQuestion(Guid sessionId)
		{
			var testSession = await _unitOfWork.TestSessionRepository.GetSession(sessionId);
			var testQuestions = await _unitOfWork.QuestionTestRepository.GetQuestionsByTestId(testSession.TestId);
			var assignedQuestions = testSession.SessionQuestions.Select(sq => sq.QuestionId).ToList();

			var availableQuestions = testQuestions
				.Where(q => !assignedQuestions.Contains(q.Id))
				.ToList();

			if (availableQuestions.Count == 0)
			{
				return Response<QuestionNextDto>.Error("No more available questions for this test");
			}

			if (!await CheckIfAnyQuestionAvailable(testSession.TestId, assignedQuestions.Count))
			{
				return Response<QuestionNextDto>.Error("No more questions for this test");
			}

			var random = new Random();
			var randomIndex = random.Next(availableQuestions.Count);
			var randomQuestion = availableQuestions[randomIndex];

			var sessionQuestion = new SessionQuestion
			{
				QuestionId = randomQuestion.Id,
				SessionId = sessionId,
				IsAnswered = false
			};

			await _unitOfWork.SessionQuestionRepository.Add(sessionQuestion);
			await _unitOfWork.Save();

			var answers = await _unitOfWork.AnswerRepository.GetByQuestionId(randomQuestion.Id);

			var questionE = new QuestionNextDto()
			{
				Title = randomQuestion.Title,
				Text = randomQuestion.Text,
				Answers = _mapper.Map<List<AnswerNextDto>>(answers)
			};

			return Response<QuestionNextDto>.Success(questionE, "New question");
		}
		public async Task<Response<SessionAnswerDto>> AnswerTheQuestion(Guid sessionId, Guid answerId)
		{
			var answer = await _unitOfWork.AnswerRepository.GetById(answerId);

			var sessionAnswer = new SessionAnswer()
			{
				AnswerId = answerId,
				SessionId = sessionId,
				IsCorrect = answer.IsCorrect
			};

			await _unitOfWork.SessionAnswerRepository.Add(sessionAnswer);
			await _unitOfWork.Save();

			return Response<SessionAnswerDto>.Success(_mapper.Map<SessionAnswerDto>(sessionAnswer), "New question");
		}

		private async Task<bool> CheckIfAnyQuestionAvailable(Guid testId, int assignedQuestionNumber)
		{
			var test = await _unitOfWork.TestRepository.GetById(testId);
			return test.QuestionsAttempted > assignedQuestionNumber;
		}

		public async Task<bool> IfGetResult(Guid sessionId)
		{
			var testSession = await _unitOfWork.TestSessionRepository.GetSession(sessionId);
			var assignedQuestions = testSession.SessionQuestions.Select(sq => sq.QuestionId).ToList();

			return !await CheckIfAnyQuestionAvailable(testSession.TestId, assignedQuestions.Count);
		}

		public async Task<Response<SessionResultDto>> GetResult(Guid sessionId)
		{
			var testSession = await _unitOfWork.TestSessionRepository.GetSession(sessionId);

			testSession.Status = TestStatus.Completed;

			_unitOfWork.TestSessionRepository.Update(testSession);
			await _unitOfWork.Save();

			var assignedQuestions = testSession.SessionQuestions.Select(sq => sq.QuestionId).ToList();
			var correctAnswers = await _unitOfWork.SessionAnswerRepository.GetCorrectAnswers(sessionId);

			var result = new SessionResultDto()
			{
				QuestionNumber = assignedQuestions.Count,
				CorrectAnswerNumber = correctAnswers.Count,
				Grade = (double)correctAnswers.Count / assignedQuestions.Count * 100
			};

			return Response<SessionResultDto>.Success(result, "Results");
		}
	}
}
