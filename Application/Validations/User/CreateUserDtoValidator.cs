using Application.DTO.User;
using FluentValidation;

namespace Application.Validations.User
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Firstname)
                .NotEmpty().WithMessage("First name can not be empty!")
                .MaximumLength(50).WithMessage("First name can not be bigger than 50 characters");
            RuleFor(x => x.Lastname)
                .NotEmpty().WithMessage("Lastname can not be empty!")
                .MaximumLength(50).WithMessage("Lastname can not be bigger than 50 characters");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email can not be empty!")
                .EmailAddress().WithMessage("Email format is not valid!");
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username can not be empty!")
                .MinimumLength(3).WithMessage("Username can not be smaller than 3 characters")
                .MaximumLength(50).WithMessage("Username can not be bigger than 50 characters");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password can not be empty!")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .MaximumLength(100).WithMessage("Password must be at most 100 characters.");
        }
    }
}
