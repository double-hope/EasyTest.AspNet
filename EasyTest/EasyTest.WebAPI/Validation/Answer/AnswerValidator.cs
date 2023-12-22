using EasyTest.Shared.DTO.Answer;
using FluentValidation;

namespace EasyTest.WebAPI.Validation.Answer
{
	public class AnswerValidator : AbstractValidator<AnswerDto>
	{
		public AnswerValidator()
		{
			RuleFor(x => x.Text)
				.NotEmpty().WithMessage("Answer text is required.")
				.MaximumLength(255).WithMessage("Answer text cannot exceed 255 characters.");

			RuleFor(x => x.IsCorrect)
				.NotNull().WithMessage("IsCorrect property is required.");
		}
	}
}
