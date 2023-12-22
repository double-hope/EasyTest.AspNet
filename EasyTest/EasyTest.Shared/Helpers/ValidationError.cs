namespace EasyTest.Shared.Helpers
{
	public class ValidationError
	{
		public string Type {  get; set; }
		public string Title { get; set; } = "One or more validation errors occurred.";
		public int Status { get; set; } = 400;
		public string TraceId { get; set; }
		public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();
	}
}
