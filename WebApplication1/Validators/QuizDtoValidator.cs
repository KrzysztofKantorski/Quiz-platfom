using FluentValidation;
using WebApplication1.Models.Dtos.admin;

namespace WebApplication1.Validators
{
    public class QuizDtoValidator: AbstractValidator<CreateQuizDto>
    {
        public QuizDtoValidator()
        {

            //Validate title field
            RuleFor(x => x.Title)
               .NotEmpty().WithMessage("You must provide title for quiz")
               .MaximumLength(50).WithMessage("Too ling quiz title");

            //Validate desctiption field
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("You must provide description for quiz")
                .MinimumLength(5).WithMessage("Quiz description must be at least 5 characters long")
                .MaximumLength(100).WithMessage("Too long quiz desctiption");
        }
    }
}
