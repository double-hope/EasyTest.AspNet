using EasyTest.Shared.Enums;

namespace EasyTest.Shared.DTO.Session
{
	public class SessionCreateDto
	{
		public Guid UserId { get; set; }
		public Guid TestId { get; set; }
	}
}
