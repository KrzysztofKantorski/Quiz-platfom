using FluentValidation;
using WebApplication1.Models.Dtos.play.Request;

namespace WebApplication1.Validators
{
    public class SubmitQuizDtoValidator: AbstractValidator<SubmitQuizDto>
    {
        public SubmitQuizDtoValidator()
        {
            RuleFor(x => x.SelectedAnswers)

                //Empty answer
                .NotEmpty().WithMessage("Answer must not be empty.")

                //Duplicated answer
                .Must(answers => answers.Select(a => a.QuestionId).Distinct().Count() == answers.Count)
                .WithMessage("Dpulicated answer.");

            //Use rules for each object
            RuleForEach(x => x.SelectedAnswers).SetValidator(new AnswerSubmissionDtoValidator());
        }
    }
}
