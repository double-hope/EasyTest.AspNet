using EasyTest.Shared.DTO.Response;
using EasyTest.Shared.DTO.Session;

namespace EasyTest.BLL.Interfaces
{
	public interface ISessionService
	{
		Task<Response<SessionDto>> Create(SessionCreateDto sessionDto);
	}
}
