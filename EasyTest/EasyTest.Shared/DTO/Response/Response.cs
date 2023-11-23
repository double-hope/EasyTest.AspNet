namespace EasyTest.Shared.DTO.Response
{
    public class Response
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
        public List<string>? Errors { get; set; }
    }
}
