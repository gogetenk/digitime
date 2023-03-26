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
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), typeof(GetCalendarQuery).Assembly));

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
builder.Services.AddApplication();

builder.WebHost.UseSentry(o =>
{
    o.Dsn = "https://13ac4b7a36c54dcd9783ba87b131534c@o4504886273835008.ingest.sentry.io/4504906196123648";
    // When configuring for the first time, to see what the SDK is doing:
    o.Debug = true;
    // Set TracesSampleRate to 1.0 to capture 100% of transactions for performance monitoring.
    // We recommend adjusting this value in production.
    o.TracesSampleRate = 1.0;
});

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
        c.OAuthAppName($"Auth0 Digitime API {app.Environment} (Test Application)");
        c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "audience", builder.Configuration["Authentication:Schemes:Bearer:ValidAudience"] } });
        c.OAuthUsePkce();
    });
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSentryTracing();
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
