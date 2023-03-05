using Microsoft.OpenApi.Models;

namespace Digitime.Server.OpenApiSecurity;

public class OpenApiBearerSecurityRequirement : OpenApiSecurityRequirement
{
    public OpenApiBearerSecurityRequirement(OpenApiSecurityScheme securityScheme)
    {
        this.Add(securityScheme, new[] { "Bearer" });
    }
}
