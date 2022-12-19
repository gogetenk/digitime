using System.Threading.Tasks;
using Digitime.Server.Domain.Users;

namespace Digitime.Server.Application.Abstractions;

public interface IUserRepository
{
    Task<User> GetbyIdAsync(string id);
    Task<User> GetbyExternalIdAsync(string id);
    Task<User> GetByEmail(string email);
    Task UpdateAsync(User user);
    Task DeleteAsync(string id);
}
