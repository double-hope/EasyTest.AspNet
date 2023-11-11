﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EasyTest.DAL.Entities
{
	public class Test
	{
		[Key]
		public Guid Id { get; set; }
		[Required]
		[DisplayName("Test Name")]
		public string Title { get; set; }
		[Required]
		[DisplayName("Test Decription")]
		public string Description { get; set; }
		public List<QuestionTest> QuestionTests { get; set; } = new();
	}
}