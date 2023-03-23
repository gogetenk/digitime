using Digitime.Server.Infrastructure.Entities;
using Digitime.Shared.Contracts.Projects;
using Digitime.Shared.Contracts.Timesheets;
using Mapster;

namespace Digitime.Server.Mappings;

public class MappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Auth0.ManagementApi.Models.User, UserEntity>()
            .Ignore(x => x.Id)
            .Map(dest => dest.ExternalId, src => src.UserId)
            .Map(dest => dest.Lastname, src => src.LastName)
            .Map(dest => dest.Firstname, src => src.FirstName)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.ProfilePicture, src => src.Picture)
            .MapToConstructor(true)
            .TwoWays();

        config.NewConfig<UserEntity, Domain.Users.User>()
           .Map(dest => dest.ExternalId, src => src.ExternalId)
           .Map(dest => dest.Lastname, src => src.Lastname)
           .Map(dest => dest.Firstname, src => src.Firstname)
           .Map(dest => dest.Email, src => src.Email)
           .Map(dest => dest.ProfilePicture, src => src.ProfilePicture)
           .MapToConstructor(true)
           .TwoWays();

        config.NewConfig<ProjectEntity, Domain.Projects.Project>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Code, src => src.Code)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.WorkspaceId, src => src.WorkspaceId)
            .Map(dest => dest.Members, src => src.Members.AsReadOnly())
            .MapToConstructor(true)
            .TwoWays();

        config.NewConfig<WorkspaceEntity, Domain.Workspaces.Workspace>()
           .Map(dest => dest.Id, src => src.Id)
           .Map(dest => dest.Name, src => src.Name)
           .Map(dest => dest.Description, src => src.Description)
           .Map(dest => dest.Members, src => src.Members.AsReadOnly())
           .MapToConstructor(true)
           .TwoWays();

        config.NewConfig<Domain.Projects.Project, ProjectDto>()
           .Map(dest => dest.Id, src => src.Id)
           .Map(dest => dest.Title, src => src.Title)
           .Map(dest => dest.Code, src => src.Code)
           .Map(dest => dest.Description, src => src.Description)
           .Map(dest => dest.WorkspaceId, src => src.WorkspaceId)
           .Map(dest => dest.Members, src => src.Members)
           .MapToConstructor(true)
           .TwoWays();

        config.NewConfig<TimesheetEntity, Domain.Timesheets.Timesheet>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Worker, src => src.Worker)
            .Map(dest => dest.UpdateDate, src => src.UpdateDate)
            .Map(dest => dest.CreateDate, src => src.CreateDate)
            .Map(dest => dest.TimesheetEntries, src => src.TimesheetEntries)
            .MapToConstructor(true)
            .TwoWays();

        config.NewConfig<TimesheetEntryEntity, Domain.Timesheets.Entities.TimesheetEntry>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Date, src => src.Date)
            .Map(dest => dest.Hours, src => src.Hours)
            .Map(dest => dest.Project, src => src.Project)
            .Map(dest => dest.Status, src => src.Status)
            .MapToConstructor(true)
            .TwoWays();

        config.NewConfig<Domain.Timesheets.Entities.TimesheetEntry, CreateTimesheetEntryReponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Date, src => src.Date)
            .Map(dest => dest.Hours, src => src.Hours)
            .Map(dest => dest.Project, src => src.Project)
            .Map(dest => dest.Status, src => src.Status)
            .MapToConstructor(true);
    }
}
