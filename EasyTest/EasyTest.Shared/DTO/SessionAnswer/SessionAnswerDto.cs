namespace EasyTest.Shared.DTO.Answer
{
	public class SessionAnswerDto
	{
		public Guid AnswerId { get; set; }
		public Guid SessionId { get; set; }
		public bool IsCorrect { get; set; }
	}
}
