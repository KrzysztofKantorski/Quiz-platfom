using FluentValidation;
using WebApplication1.Models.Dtos.admin;
using WebApplication1.Models.Dtos.auth;

namespace WebApplication1.Validators
{
    public class UpdateQuestionDtoValidator: AbstractValidator<UpdateQuestionDto>
    {
        public UpdateQuestionDtoValidator()
        {
            //Validate text field
            RuleFor(x => x.Text)
               .NotEmpty().WithMessage("You must provide text for question")
               .MinimumLength(5).WithMessage("Incorrect question")
               .MaximumLength(30).WithMessage("Too long question");

            //Validate points field
            RuleFor(x => x.Points)
                .GreaterThan(0).WithMessage("You must provide value for points greater than 0");

            //Validate answers field
            RuleFor(x => x.Answers)
                .NotEmpty().WithMessage("You must provide answers.")
                .Must(answers => answers.Count >= 2 && answers.Count <= 6)
                .WithMessage("Question must have between 2 and 6 answers.")
                .Must(answers => answers.Count(a => a.IsCorrect) == 1)
                .WithMessage("There must be exactly one correct answer.");

            //Validate answers from list
            RuleForEach(x => x.Answers).ChildRules(answer =>
            {
                answer.RuleFor(a => a.Text)
                    .NotEmpty().WithMessage("Answer text cannot be empty.")
                    .MaximumLength(100).WithMessage("Answer text is too long.");
            });
        }
    }
}
