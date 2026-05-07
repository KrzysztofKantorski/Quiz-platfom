using FluentValidation;
using WebApplication1.Models.Dtos.auth;

namespace WebApplication1.Validators
{
    public class RegisterDtoValidator: AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator() {

            //Validate email field
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("You must provide email address")
                .EmailAddress().WithMessage("Wrong email format");

            //Validate password field
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("You must provide password")
                .MinimumLength(8).WithMessage("Password must have at least 8 characters")
                .Matches("[A-Z]").WithMessage("Password must have at least one capital letter")
                .Matches("[0-9]").WithMessage("Password must have at least one number");
        }
    }
}
