using System;
using System.Text.RegularExpressions;
using FluentValidation;

namespace Digitime.Server.Application.Calendar.Comands;
public class CreateTimesheetEntryCommandValidator : AbstractValidator<CreateTimesheetEntryCommand>
{
    private static Regex _objectIdRegex = new(@"^[0-9a-fA-F]{24}$");
    
    public CreateTimesheetEntryCommandValidator()
    {
        RuleFor(x => x.TimesheetId)
            .Must(BeAnObjectId).WithMessage("TimesheetId must be a valid Id").When(x => x.TimesheetId is not null);
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("TimesheetId is required")
            .Must(BeAnObjectId).WithMessage("UserId must be a valid Id");

        RuleFor(x => x.TimesheetEntry)
            .NotNull().WithMessage("TimesheetEntry is required");
        RuleFor(x => x.TimesheetEntry.Project.Title)
            .NotEmpty().WithMessage("Project Title is required")
            .MaximumLength(100).WithMessage("Project Title must be 100 characters or less");
        RuleFor(x => x.TimesheetEntry.Project.Id)
            .NotEmpty().WithMessage("Project Id is required")
            .Must(BeAnObjectId).WithMessage("Project Id must be a valid Id");
        RuleFor(x => x.TimesheetEntry.Hours)
            .NotEmpty().WithMessage("Hours is required")
            .InclusiveBetween(1, 24).WithMessage("Hours must be between 1 and 24");
    }
    private bool BeAnObjectId(string value) => _objectIdRegex.Match(value).Success;
}
