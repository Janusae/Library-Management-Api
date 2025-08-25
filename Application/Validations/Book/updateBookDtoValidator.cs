using Application.DTO.BookDto;
using FluentValidation;

namespace Application.Validations.Book
{
    public class updateBookDtoValidator : AbstractValidator<UpdateBookDto>
    {
        public updateBookDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id can not be empty!")
                .Must(id => int.TryParse(id, out _)).WithMessage("Id must be a valid number.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name can not be empty!")
                .MinimumLength(2).WithMessage("Name can not be smaller than 2 characters.")
                .MaximumLength(100).WithMessage("Name can not be bigger than 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description can not be empty!")
                .MinimumLength(2).WithMessage("Description can not be smaller than 2 characters.")
                .MaximumLength(300).WithMessage("Description can not be bigger than 300 characters.");
        }
    }
}
