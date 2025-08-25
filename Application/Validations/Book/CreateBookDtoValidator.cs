using Application.DTO.BookDto;
using FluentValidation;

namespace Application.Validations.Book
{
    public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
    {
        public CreateBookDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name can not be empty!")
                .MinimumLength(2).WithMessage("Name can not be smaller than 2 characters")
                .MaximumLength(30).WithMessage("Name can not be bigger than 30 characters");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description can not be empty!")
                .MinimumLength(2).WithMessage("Description can not be smaller than 2 characters")
                .MaximumLength(100).WithMessage("Description can not be bigger than 100 characters");
        }
    }
}
