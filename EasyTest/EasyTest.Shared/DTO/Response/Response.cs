namespace EasyTest.Shared.DTO.Response
{
    public class Response<T>
    {
        public Status Status { get; set; }
        public string Message { get; set; }
        public T? data { get; set; }
        public List<string>? Errors { get; set; }
    }
    public enum Status
    {
        Success,
        Error
    }
}
