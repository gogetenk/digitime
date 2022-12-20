using System.Text.RegularExpressions;
using FluentValidation;

namespace Digitime.Server.Application.Timesheets.Commands;
public class CreateTimesheetEntryCommandValidator : AbstractValidator<CreateTimesheetEntryCommand>
{
    private static Regex _objectIdRegex = new(@"^[0-9a-fA-F]{24}$");

    public CreateTimesheetEntryCommandValidator()
    {
        RuleFor(x => x.TimesheetId)
            .Must(BeAnObjectId).WithMessage("TimesheetId must be a valid Id").When(x => x.TimesheetId is not null);

        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("Project Id is required")
            .Must(BeAnObjectId).WithMessage("Project Id must be a valid Id");
        RuleFor(x => x.Hours)
            .NotEmpty().WithMessage("Hours is required")
            .InclusiveBetween(1, 24).WithMessage("Hours must be between 1 and 24");
    }
    private bool BeAnObjectId(string value) => _objectIdRegex.Match(value).Success;
}
