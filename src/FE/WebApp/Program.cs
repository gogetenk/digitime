using Blazored.LocalStorage;
using Digitime.Client;
using Digitime.Client.Authentication;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Services.AddBlazoredLocalStorage();
var backendBaseAddress = builder.Configuration.GetValue<string>("BackendBaseUri");
builder.Services.AddHttpClient(
        "DigitimeApi",
        client => client.BaseAddress = new Uri(backendBaseAddress)
    )
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services
    .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("DigitimeApi"));

builder.Services.AddAuthorizationCore();
builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);
    options.ProviderOptions.DefaultScopes.Add("openid profile");
});

builder.Services.AddApiAuthorization()
                .AddAccountClaimsPrincipalFactory<RolesClaimsPrincipalFactory>();

var host = builder.Build();
var logger = host.Services
    .GetRequiredService<ILoggerFactory>()
    .CreateLogger<Program>();

logger.LogInformation("App initialized.");
logger.LogInformation($"Backend URI is set to {backendBaseAddress} and environment set to {builder.HostEnvironment.Environment}.");

await host.RunAsync();
