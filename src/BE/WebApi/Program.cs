using System.Reflection;
using Digitime.Server;
using Digitime.Server.Application.Calendar.Queries;
using Digitime.Server.Infrastructure;
using Digitime.Server.Middlewares;
using Digitime.Server.OpenApiSecurity;
using Digitime.Server.Settings;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(Digitime.Server.Queries.GetCalendarQuery).Assembly);
builder.Services.AddAuthentication()
    .AddJwtBearer();
//.AddCookie("cookie")
//.AddOAuth("oidc", config =>
//{
//    config.ClientId = "xL02wLlcePvYOlm04nvRP0kk7hpYbSdl";
//    config.ClientSecret = "dfuNih_kkvIvEx-EjQQaKw-43PIEfqjHI1DEMGrUMbCp_Jgc4CfSFq9iY91xQIRW";
//    config.CallbackPath = new PathString("/");
//    config.AuthorizationEndpoint = "https://digitime-dev.eu.auth0.com/authorize";
//    config.TokenEndpoint = "https://digitime-dev.eu.auth0.com/oauth/token";
//    config.UserInformationEndpoint = "https://digitime-dev.eu.auth0.com/userinfo";
//    config.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents()
//    {
//        OnCreatingTicket = async (c) =>
//        {
//            var t = c.AccessToken;
//        }
//    };
//});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GetTimesheet", policy =>
                      policy.RequireClaim("permissions", "timesheet:create"));
});

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<GetCalendarQueryValidator>())
    .AddNewtonsoftJson();

builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DigitimeAPI", Version = "v1" });
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
//    {
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = ParameterLocation.Header,
//        Description = "JWT Authorization header using the Bearer scheme."
//    });
//    //c.AddSecurityDefinition("oauth", new OpenApiSecurityScheme
//    //{
//    //    Name = "Authorization",
//    //    In = ParameterLocation.Header,
//    //    Type = SecuritySchemeType.OpenIdConnect,
//    //    Flows = new OpenApiOAuthFlows
//    //    {
//    //        Implicit = new OpenApiOAuthFlow
//    //        {
//    //            Scopes = new Dictionary<string, string>
//    //            {
//    //                { "openid", "Open Id" }
//    //            },
//    //            AuthorizationUrl = new Uri("https://digitime-dev.eu.auth0.com/authorize", UriKind.Absolute)
//    //        },
//    //        AuthorizationCode = new OpenApiOAuthFlow
//    //        {
//    //            AuthorizationUrl = new Uri("https://digitime-dev.eu.auth0.com/authorize"),
//    //            TokenUrl = new Uri("https://digitime-dev.eu.auth0.com/oauth/token"),
//    //            Scopes = new Dictionary<string, string>
//    //            {
//    //                {"api1", "Demo API - full access"}
//    //            }
//    //        }
//    //    }
//    //});
//    //c.AddSecurityDefinition("oidc", new OpenApiSecurityScheme
//    //{
//    //    Type = SecuritySchemeType.OpenIdConnect,
//    //    OpenIdConnectUrl = new Uri("https://digitime-dev.eu.auth0.com/.well-known/openid-configuration", UriKind.Absolute),
//    //    Flows = new OpenApiOAuthFlows
//    //    {
//    //        ClientCredentials = new OpenApiOAuthFlow
//    //        {
//    //            AuthorizationUrl = new Uri("https://digitime-dev.eu.auth0.com/authorize", UriKind.Absolute),
//    //            TokenUrl = new Uri("https://digitime-dev.eu.auth0.com/oauth/token", UriKind.Absolute),
//    //            Scopes = new Dictionary<string, string>
//    //            {
//    //                { "readAccess", "Access read operations" },
//    //                { "writeAccess", "Access write operations" }
//    //            }
//    //        },
//    //        AuthorizationCode = new OpenApiOAuthFlow
//    //        {
//    //            AuthorizationUrl = new Uri("https://digitime-dev.eu.auth0.com/authorize"),
//    //            TokenUrl = new Uri("https://digitime-dev.eu.auth0.com/oauth/token"),
//    //            Scopes = new Dictionary<string, string>
//    //            {
//    //                {"api1", "Demo API - full access"}
//    //            }
//    //        }
//    //    }
//    //});

//    //c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    //{
//    //    {
//    //        new OpenApiSecurityScheme
//    //        {
//    //            Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme,Id = "jwt"}
//    //        },
//    //        new string[] {}
//    //    }
//    //});
//    //c.OperationFilter<SecurityRequirementsOperationFilter>();
//});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyProject", Version = "v1.0.0" });

    string securityDefinitionName = "oauth2" ?? "Bearer";
    OpenApiSecurityScheme securityScheme = new OpenApiBearerSecurityScheme();
    OpenApiSecurityRequirement securityRequirement = new OpenApiBearerSecurityRequirement(securityScheme);

    if (securityDefinitionName.ToLower() == "oauth2")
    {
        securityScheme = new OpenApiOAuthSecurityScheme("https://digitime-dev.eu.auth0.com", "https://dev.digitime.app");
        securityRequirement = new OpenApiOAuthSecurityRequirement();
    }

    c.AddSecurityDefinition(securityDefinitionName, securityScheme);
    c.AddSecurityRequirement(securityRequirement);
});


builder.Services.AddEasyCaching(options =>
{
    //use memory cache that named default
    options.UseInMemory("memory");
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
});

// Services
builder.Services.AddApi();
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    //app.UseSwaggerUI();
    //{
    //c.OAuthClientId("xL02wLlcePvYOlm04nvRP0kk7hpYbSdl");
    //c.OAuthClientSecret("dfuNih_kkvIvEx-EjQQaKw-43PIEfqjHI1DEMGrUMbCp_Jgc4CfSFq9iY91xQIRW");
    //c.OAuthAppName("Auth0 Digitime API Dev (Test Application)");
    //c.OAuthScopeSeparator(" ");
    //c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "foo", "bar" } });
    //c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
    //c.EnablePersistAuthorization()
    //});
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Glossary v1");

            c.OAuthClientId("xL02wLlcePvYOlm04nvRP0kk7hpYbSdl");
            c.OAuthClientSecret("dfuNih_kkvIvEx-EjQQaKw-43PIEfqjHI1DEMGrUMbCp_Jgc4CfSFq9iY91xQIRW");
            c.OAuthAppName("Auth0 Digitime API Dev (Test Application)");
            c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "audience", "https://dev.digitime.app" } });
            c.OAuthUsePkce();
    });
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(x => x
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(origin => true) // allow any origin
              .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

public partial class Program // Needed for IntegrationTests
{ }
