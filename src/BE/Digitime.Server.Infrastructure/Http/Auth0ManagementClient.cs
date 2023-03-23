using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Digitime.Server.Infrastructure.Entities;
using DnsClient.Internal;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Digitime.Server.Infrastructure.Http;

public class Auth0ManagementClient
{
    private static ManagementApiClient _managementApiClient;
    private readonly IConfiguration _config;
    private readonly ILogger<Auth0ManagementClient> _logger;

    public Auth0ManagementClient(IConfiguration config, ILogger<Auth0ManagementClient> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<UserEntity> GetByEmail(string email)
    {
        try
        {
            if (_managementApiClient == null)
                await Authenticate();

            _logger.LogDebug("Authentication to Auth0 Management API successful.");
            var users = await _managementApiClient.Users.GetUsersByEmailAsync(email);
            _logger.LogDebug("Fetched users from the Auth0 Management API.");

            if (users.Count() == 0)
            {
                _logger.LogWarning($"No user found with email {email}.");
                return null;
            }

            if (users.Count() > 1)
            {
                _logger.LogWarning($"Multiple users found with email {email}. Returning the first one.");
            }

            var user = users.First();
            if ((string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName)) && !string.IsNullOrEmpty(user.FullName))
            {
                user.FirstName = user.FullName.Split(' ')?[0];
                user.LastName = user.FullName.Split(' ')?[1];
            }

            return user.Adapt<UserEntity>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting user by email from Auth0.");
            return null;
        }
    }

public async Task<UserEntity> GetById(string id)
    {
        try
        {
            if (_managementApiClient == null)
                await Authenticate();

            _logger.LogDebug("Authentication to Auth0 Management API successful.");
            var user = await _managementApiClient!.Users.GetAsync(id);
            _logger.LogDebug("Found the user on the Auth0 Management API.");
            if ((string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName)) && !string.IsNullOrEmpty(user.FullName))
            {
                user.FirstName = user.FullName.Split(' ')?[0];
                user.LastName = user.FullName.Split(' ')?[1];
            }
            return user.Adapt<UserEntity>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting user by id from Auth0.");
            return null;
        }
    }

    private async Task Authenticate()
    {
        _logger.LogDebug($"Authenticating to Auth0 Management API... Url : {_config["ExternalApis:Auth0ManagementApi:Url"]}. ClientId : {_config["ExternalApis:Auth0ManagementApi:ClientId"]}");
        var authClient = new AuthenticationApiClient(new Uri(_config["ExternalApis:Auth0ManagementApi:Url"]));
        var token = await authClient.GetTokenAsync(new ClientCredentialsTokenRequest()
        {
            ClientId = _config["ExternalApis:Auth0ManagementApi:ClientId"],
            ClientSecret = _config["ExternalApis:Auth0ManagementApi:ClientSecret"],
            Audience = _config["ExternalApis:Auth0ManagementApi:Audience"]
        });
        _logger.LogDebug("Received a token from Auth0 Management API.");
        _managementApiClient = new ManagementApiClient(token.AccessToken, new Uri(_config["ExternalApis:Auth0ManagementApi:Audience"]));
    }
}
