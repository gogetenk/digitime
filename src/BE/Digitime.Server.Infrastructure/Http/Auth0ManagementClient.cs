using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Digitime.Server.Infrastructure.Entities;
using Mapster;
using Microsoft.Extensions.Configuration;

namespace Digitime.Server.Infrastructure.Http;

public class Auth0ManagementClient
{
    private ManagementApiClient _managementApiClient;
    private readonly IConfiguration _config;

    public Auth0ManagementClient(IConfiguration config)
    {
        _config = config;
    }

    private async Task Authenticate()
    {
        var authClient = new AuthenticationApiClient(new Uri("https://digitime-dev.eu.auth0.com"));
        var token = await authClient.GetTokenAsync(new ClientCredentialsTokenRequest()
        {
            ClientId = _config["ExternalApis:Auth0ManagementApi:ClientId"],
            ClientSecret = _config["ExternalApis:Auth0ManagementApi:ClientId"],
            Audience = _config["ExternalApis:Auth0ManagementApi:Audience"]
        });
        _managementApiClient = new ManagementApiClient(token.AccessToken, new Uri(_config["ExternalApis:Auth0ManagementApi:Url"]));
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
