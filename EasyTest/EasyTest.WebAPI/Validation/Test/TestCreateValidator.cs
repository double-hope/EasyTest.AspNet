using EasyTest.Shared.DTO.Test;
using FluentValidation;

namespace EasyTest.WebAPI.Validation.Test
{
	public class TestCreateValidator : AbstractValidator<TestCreateDto>
	{
        public TestCreateValidator()
        {
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(255).WithMessage("Title cannot exceed 255 characters.");

			RuleFor(x => x.Description)
				.NotEmpty().WithMessage("Description is required.")
				.MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
		}
	}
}
