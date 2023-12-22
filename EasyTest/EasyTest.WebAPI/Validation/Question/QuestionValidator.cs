using EasyTest.Shared.DTO.Question;
using EasyTest.WebAPI.Validation.Answer;
using FluentValidation;

namespace EasyTest.WebAPI.Validation.Question
{
	public class QuestionValidator : AbstractValidator<QuestionDto>
	{
		public QuestionValidator()
		{
			RuleFor(x => x.Title)
				.NotEmpty().WithMessage("Title is required.")
				.MaximumLength(255).WithMessage("Title cannot exceed 255 characters.");

			RuleFor(x => x.Text)
				.NotEmpty().WithMessage("Text is required.")
				.MaximumLength(1000).WithMessage("Text cannot exceed 1000 characters.");

			RuleFor(x => x.Answers)
				.Must(answers => answers != null && answers.Any())
				.WithMessage("At least one answer is required.");

			RuleForEach(x => x.Answers)
				.SetValidator(new AnswerValidator());
		}
	}
}
