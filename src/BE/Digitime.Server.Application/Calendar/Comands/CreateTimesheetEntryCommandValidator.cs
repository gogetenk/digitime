using System;
using FluentValidation;

namespace Digitime.Server.Application.Calendar.Comands;
public class CreateTimesheetEntryCommandValidator : AbstractValidator<CreateTimesheetEntryCommand>
{
    public CreateTimesheetEntryCommandValidator()
    {
        RuleFor(x => x.TimesheetId)
            .NotEmpty().WithMessage("TimesheetId is required")
            .Must(BeAGuid).WithMessage("TimesheetId must be a valid GUID");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("TimesheetId is required")
            .Must(BeAGuid).WithMessage("UserId must be a valid GUID");

        RuleFor(x => x.TimesheetEntry)
            .NotNull().WithMessage("TimesheetEntry is required");
        RuleFor(x => x.TimesheetEntry.ProjectTitle)
            .NotEmpty().WithMessage("ProjectTitle is required")
            .MaximumLength(100).WithMessage("ProjectTitle must be 100 characters or less");
        RuleFor(x => x.TimesheetEntry.ProjectId)
            .NotEmpty().WithMessage("ProjectId is required")
            .Must(BeAGuid).WithMessage("ProjectId must be a valid GUID");
    }

    private bool BeAGuid(string value) => Guid.TryParse(value, out _);
}
