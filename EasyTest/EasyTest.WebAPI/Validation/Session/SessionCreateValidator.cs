using FluentValidation;

using EasyTest.Shared.DTO.Session;

namespace EasyTest.WebAPI.Validation.Session
{
	public class SessionCreateValidator : AbstractValidator<SessionCreateDto>
	{
        public SessionCreateValidator()
        {
			RuleFor(x => x.TestId)
				.NotEmpty().WithMessage("Test ID is required.");
		}
    }
}
