using FluentValidation;
using WebApplication1.Models.Dtos.auth;

namespace WebApplication1.Validators
{
    public class LoginDtoValidator: AbstractValidator<LoginDto>
    {
        public LoginDtoValidator() {

            //Validate email field
            RuleFor(x => x.Email)
               .NotEmpty().WithMessage("You must provide email address")
               .EmailAddress().WithMessage("Wrong email format");

            //Validate password field
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("You must provide password");
        }
    }
}
