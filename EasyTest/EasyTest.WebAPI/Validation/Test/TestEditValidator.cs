using EasyTest.Shared.DTO.Test;
using FluentValidation;

namespace EasyTest.WebAPI.Validation.Test
{
	public class TestEditValidator : AbstractValidator<TestEditDto>
	{
        public TestEditValidator()
        {
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(255).WithMessage("Title cannot exceed 255 characters.");

			RuleFor(x => x.Description)
				.NotEmpty().WithMessage("Description is required.")
				.MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
			
			RuleFor(x => x.NumberOfAttempts)
				.GreaterThan(0).WithMessage("Number of attempts must be greater than 0.");

			RuleFor(x => x.QuestionsAttempted)
				.GreaterThanOrEqualTo(0).WithMessage("Questions attempted cannot be less than 0.");

		}
	}
}
