using AutoMapper;
using EasyTest.BLL.Interfaces;
using EasyTest.DAL.Entities;
using EasyTest.DAL.Repository.IRepository;
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
	}
}
