using Digitime.Server.Queries;
using FluentValidation;
using System.Linq;

namespace Digitime.Server.Application.Calendar.Queries;
public class GetCalendarQueryValidator : AbstractValidator<GetCalendarQuery>
{
    public GetCalendarQueryValidator()
    {
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(2).WithMessage("Country must be 2 characters long")
            .Must(x => x.All(char.IsLetter)).WithMessage("Country must be a valid ISO 3166-1 alpha-2 code");
        RuleFor(x => x.Month)
            .NotEmpty().WithMessage("Month is required")
            .InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12")
            .Must(x => x % 1 == 0).WithMessage("Month must be an integer");
        RuleFor(x => x.Year)
            .NotEmpty().WithMessage("Year is required")
            .InclusiveBetween(1900, 2100).WithMessage("Year must be between 1900 and 2100")
            .Must(x => x % 1 == 0).WithMessage("Year must be an integer");
    }
}
