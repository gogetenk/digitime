using Digitime.Server.Queries;
using FluentValidation;

namespace Digitime.Server.Application.Calendar.Queries;
public class GetCalendarQueryValidator : AbstractValidator<GetCalendarQuery>
{
    public GetCalendarQueryValidator()
    {
        RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required");
        RuleFor(x => x.Month).NotEmpty().WithMessage("Month is required");
        RuleFor(x => x.Year).NotEmpty().WithMessage("Year is required");
    }
}
