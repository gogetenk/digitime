using Digitime.Shared.Contracts.Projects;

namespace Digitime.Shared.UI.ViewModels;

public record AddTimesheetEntryFormVM(ProjectDto Project, int Hours);
