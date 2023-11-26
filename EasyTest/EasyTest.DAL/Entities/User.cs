using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyTest.DAL.Entities
{
	public class User : IdentityUser<Guid>
	{
		public required string Name { get; set; }
		[NotMapped]
		public string Role { get; set; }
	}
}
