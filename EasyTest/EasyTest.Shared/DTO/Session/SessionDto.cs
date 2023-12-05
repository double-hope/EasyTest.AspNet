using EasyTest.Shared.Enums;

namespace EasyTest.Shared.DTO.Session
{
	public class SessionDto
	{
		public Guid UserId { get; set; }
		public Guid TestId { get; set; }
		public TestStatus Status { get; set; }
	}
}
