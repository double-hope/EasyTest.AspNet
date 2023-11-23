﻿namespace EasyTest.Shared.DTO.Response
{
    public class Response
    {
        public Status Status { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
        public List<string>? Errors { get; set; }
    }
    public enum Status
    {
        Success,
        Error
    }
}
