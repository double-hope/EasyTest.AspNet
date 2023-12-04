﻿namespace EasyTest.Shared.DTO.Test
{
    public class TestEditDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int NumberOfAttempts { get; set; } = 1;
    }
}
