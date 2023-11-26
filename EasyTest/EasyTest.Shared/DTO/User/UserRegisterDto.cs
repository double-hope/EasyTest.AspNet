using EasyTest.Shared.Enums;

namespace EasyTest.Shared.DTO.User
{
    public class UserRegisterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRoles Role { get; set; }
    }
}
