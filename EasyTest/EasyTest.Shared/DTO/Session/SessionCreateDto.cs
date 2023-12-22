using EasyTest.Shared.Enums;

namespace EasyTest.Shared.DTO.Session
{
	public class SessionCreateDto
	{
		public string UserEmail { get; set; }
		public Guid TestId { get; set; }
	}
}
