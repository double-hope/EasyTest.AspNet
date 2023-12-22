using EasyTest.Shared.DTO.User;
using FluentValidation;

namespace EasyTest.WebAPI.Validation.Auth
{
	public class UserRegisterValidator : AbstractValidator<UserRegisterDto>
	{
        public UserRegisterValidator()
        {
			RuleFor(x => x.Name)
				.NotEmpty()
				.MinimumLength(6)
				.WithMessage("Username is too short. Minimum length is 6 symbols.");

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("Invalid email address.");

			RuleFor(x => x.Password)
				.NotEmpty()
				.MinimumLength(6).WithMessage("Password is too short. Minimum length is 6 symbols.")
				.Matches("^(?=.*[A-Za-z])(?=.*\\d).+").WithMessage("Password must contain both letters and digits.")
				.Must(password => password.Any(char.IsUpper)).WithMessage("Passwords must have at least one uppercase ('A'-'Z').")
				.Must(password => password.Any(ch => !char.IsLetterOrDigit(ch))).WithMessage("Passwords must have at least one non-alphanumeric character.");

			RuleFor(x => x.Role)
				.NotEmpty().WithMessage("Role is required.")
				.IsInEnum().WithMessage("Invalid role.");
		}
	}
}
