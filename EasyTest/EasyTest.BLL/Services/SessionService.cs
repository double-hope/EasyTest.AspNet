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
			var inProgressSession = await _unitOfWork.TestSessionRepository.GetInProgressSession(sessionDto.UserId, sessionDto.TestId);
			
			if(inProgressSession != null)
			{
				return Response<SessionDto>.Success(_mapper.Map<SessionDto>(inProgressSession), "Return session created early");
			}
			var sessionE = _mapper.Map<TestSession>(sessionDto);

			sessionE.Status = TestStatus.InProgress;
			await _unitOfWork.TestSessionRepository.Add(sessionE);
			await _unitOfWork.Save();

			return Response<SessionDto>.Success(_mapper.Map<SessionDto>(sessionE), "Session created successfully");
		}

		public async Task<Response<QuestionDto>> NextQuestion(Guid sessionId)
		{
			var testSession = await _unitOfWork.TestSessionRepository.GetSession(sessionId);
			var testQuestions = await _unitOfWork.QuestionTestRepository.GetQuestionsByTestId(testSession.TestId);
			var assignedQuestions = testSession.SessionQuestions.Select(sq => sq.QuestionId).ToList();

			var availableQuestions = testQuestions
				.Where(q => !assignedQuestions.Contains(q.Id))
				.ToList();

			if (availableQuestions.Count == 0)
			{
				return Response<QuestionDto>.Error("No more questions for this test");
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

			var questionE = new QuestionDto()
			{
				Title = randomQuestion.Title,
				Text = randomQuestion.Text,
				Answers = _mapper.Map<List<AnswerDto>>(answers)
			};

			return Response<QuestionDto>.Success(questionE, "New question");
		}
	}
}
