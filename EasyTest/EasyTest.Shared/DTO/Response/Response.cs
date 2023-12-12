using EasyTest.Shared.Constants;

namespace EasyTest.Shared.DTO.Response
{
    public class Response<T>
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

		public static Response<T> Success(T data, string? message = null)
		{
			return new Response<T> { Status = ResponseStatusCodesConst.Success, Data = data, Message = message ?? string.Empty };
		}

		public static Response<T> Error(string message, List<string>? errors = null)
		{
			return new Response<T> { Status = ResponseStatusCodesConst.Error, Message = message, Errors = errors };
		}
	}
}
