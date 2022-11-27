using AutoMapper;

namespace Digitime.Server.Infrastructure.Mapping;

public interface IMapTo<T>
{
    void Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(T));
}