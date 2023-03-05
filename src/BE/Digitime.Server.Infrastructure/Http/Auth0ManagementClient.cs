using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Digitime.Server.Infrastructure.Entities;
using Mapster;

namespace Digitime.Server.Infrastructure.Http;

public class Auth0ManagementClient
{
    private ManagementApiClient _managementApiClient;

    public Auth0ManagementClient()
    {
    }

    private async Task Authenticate()
    {
        var authClient = new AuthenticationApiClient(new Uri("https://digitime-dev.eu.auth0.com"));
        var token = await authClient.GetTokenAsync(new ClientCredentialsTokenRequest()
        {
            ClientId = "6qkUkFnMGY6hLceMRM5emwHuqwgVrkJf",
            ClientSecret = "5t1Uk5TIzqWGQnNy9fjrXLbZJQfdYbSqsgwwoye9BXoOplfPlkjW7gbYD0UPTIHd",
            Audience = "https://digitime-dev.eu.auth0.com/api/v2/"
        });
        _managementApiClient = new ManagementApiClient(token.AccessToken, new Uri("https://digitime-dev.eu.auth0.com/api/v2"));
    }

    public async Task<UserEntity> GetByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<UserEntity> GetById(string id)
    {
        try
        {
            if (_managementApiClient == null)
                await Authenticate();

            var user = await _managementApiClient.Users.GetAsync(id);
            if ((string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName)) && !string.IsNullOrEmpty(user.FullName))
            {
                user.FirstName = user.FullName.Split(' ')?[0];
                user.LastName = user.FullName.Split(' ')?[1];
            }
            return user.Adapt<UserEntity>();
        }
        catch (Exception e)
        {
            return null;
        }
    }
}
