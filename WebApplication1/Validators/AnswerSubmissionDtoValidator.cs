using FluentValidation;
using WebApplication1.Models.Dtos.play.Request;

namespace WebApplication1.Validators
{
    public class AnswerSubmissionDtoValidator: AbstractValidator<AnswerSubmissionDto>
    {
        public AnswerSubmissionDtoValidator()
        {
            RuleFor(x => x.QuestionId)
                .GreaterThan(0).WithMessage("Question id must be greater than 0");

            RuleFor(x => x.SelectedAnswerId)
                .GreaterThan(0).WithMessage("Answer id must be greater than 0");
        }
    }
}
