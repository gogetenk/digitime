using System.Reflection;
using Digitime.Server;
using Digitime.Server.Application.Timesheets.Queries;
using Digitime.Server.Infrastructure;
using Digitime.Server.Middlewares;
using Digitime.Server.OpenApiSecurity;
using Digitime.Server.Settings;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(GetCalendarQuery).Assembly);
builder.Services
    .AddAuthentication()
    .AddJwtBearer();

builder.Services.AddAuthorization();
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson();

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Digitime API", Version = "v1.0.0" });

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
//builder.Services.AddHttpClient("PublicHolidaysClient", client => CreateMockHttpClient());

builder.Services.AddEasyCaching(options => options.UseInMemory("memory"));
builder.Services.AddHttpContextAccessor();
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
{
    //public static HttpClient CreateMockHttpClient()
    //{
    //    var mockHttp = new MockHttpMessageHandler();
    //    // Mocking calls to public holidays api
    //    mockHttp.When($"*")
    //            .Respond(HttpStatusCode.OK);

    //    var client = new HttpClient(mockHttp);
    //    client.BaseAddress = new Uri("http://toto");
    //    client.MaxResponseContentBufferSize = 32;
    //    client.Timeout = TimeSpan.FromSeconds(10);
    //    return client;
    //}
}
