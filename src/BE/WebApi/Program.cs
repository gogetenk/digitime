using System.Reflection;
using Digitime.Server;
using Digitime.Server.Application.Calendar.Queries;
using Digitime.Server.Infrastructure;
using Digitime.Server.Middlewares;
using Digitime.Server.OpenApiSecurity;
using Digitime.Server.Settings;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(Digitime.Server.Queries.GetCalendarQuery).Assembly);
builder.Services
    .AddAuthentication()
    .AddJwtBearer();

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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyProject", Version = "v1.0.0" });

    string securityDefinitionName = "oauth2" ?? "Bearer";
    OpenApiSecurityScheme securityScheme = new OpenApiBearerSecurityScheme();
    OpenApiSecurityRequirement securityRequirement = new OpenApiBearerSecurityRequirement(securityScheme);

    if (securityDefinitionName.ToLower() == "oauth2")
    {
        securityScheme = new OpenApiOAuthSecurityScheme(builder.Configuration["Authentication:Schemes:Bearer:Authority"]);
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
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Digitime API v1");
        c.OAuthClientId(builder.Configuration["Authentication:Schemes:Bearer:ClientId"]);
        c.OAuthClientSecret(builder.Configuration["Authentication:Schemes:Bearer:ClientSecret"]);
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
