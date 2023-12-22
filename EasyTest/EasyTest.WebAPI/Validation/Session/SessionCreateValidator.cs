using EasyTest.Shared.DTO.Session;
using FluentValidation;

namespace EasyTest.WebAPI.Validation.Session
{
	public class SessionCreateValidator : AbstractValidator<SessionCreateDto>
	{
        public SessionCreateValidator()
        {
			RuleFor(x => x.UserEmail)
				.NotEmpty().WithMessage("User email is required.")
				.EmailAddress().WithMessage("Invalid email address format.");

			RuleFor(x => x.TestId)
				.NotEmpty().WithMessage("Test ID is required.");
		}
    }
}
