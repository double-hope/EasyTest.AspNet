﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyTest.DAL.Entities
{
	public class User : IdentityUser
	{
		[Required]
		public string Name { get; set; }
		[NotMapped]
		public string Role { get; set; }
	}
}
