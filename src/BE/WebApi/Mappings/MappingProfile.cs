using Digitime.Server.Domain.Users;
using Digitime.Server.Infrastructure.Entities;
using Mapster;

namespace Digitime.Server.Mappings;

public class MappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserEntity>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Firstname, src => src.Firstname)
            .Map(dest => dest.Lastname, src => src.Lastname)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.ProfilePicture, src => src.ProfilePicture)
            .Map(dest => dest.ExternalId, src => src.ExternalId)
            .MapToConstructor(true)
            .TwoWays();

        config.NewConfig<ProjectEntity, Domain.Projects.Project>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Code, src => src.Code)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.WorkspaceId, src => src.WorkspaceId)
            .Map(dest => dest.Members, src => src.Members)
            .MapToConstructor(true)
            .TwoWays();
    }
}
