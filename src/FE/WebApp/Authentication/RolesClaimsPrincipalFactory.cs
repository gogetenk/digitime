using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

namespace Digitime.Client.Authentication;

public class RolesClaimsPrincipalFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
{
    public RolesClaimsPrincipalFactory(IAccessTokenProviderAccessor accessor) : base(accessor)
    {
    }

    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
    {
        var user = await base.CreateUserAsync(account, options);
        if (user.Identity.IsAuthenticated)
        {
            var identity = (ClaimsIdentity)user.Identity;
            var permissionsClaims = identity.FindAll("permissions");
            if (permissionsClaims != null && permissionsClaims.Any())
            {
                foreach (var existingClaim in permissionsClaims)
                {
                    identity.RemoveClaim(existingClaim);
                }

                var permissionsElem = account.AdditionalProperties[identity.RoleClaimType];
                if (permissionsElem is JsonElement permissions)
                {
                    if (permissions.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var permission in permissions.EnumerateArray())
                        {
                            identity.AddClaim(new Claim("permissions", permission.GetString()));
                        }
                    }
                    else
                    {
                        identity.AddClaim(new Claim("permissions", permissions.GetString()));
                    }
                }
            }
        }

        return user;
    }
}