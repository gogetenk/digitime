using System.Diagnostics;
using Blazored.LocalStorage;
using Digitime.Client;
using Digitime.Client.Infrastructure.Abstractions;
using Digitime.Shared.Authentication;
using Digitime.Shared.UI.Components;
using Digitime.Shared.UI.Data;
using Digitime.Shared.UI.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sentry;

// Capture blazor bootstrapping errors
using var sdk = SentrySdk.Init(o =>
{
    o.AutoSessionTracking = true;
    o.Dsn = "https://13ac4b7a36c54dcd9783ba87b131534c@o4504886273835008.ingest.sentry.io/4504906196123648";
    // Set TracesSampleRate to 1.0 to capture 100% of transactions for performance monitoring.
    // We recommend adjusting this value in production.
    o.TracesSampleRate = 1.0;
    //IsGlobalModeEnabled will be true for Blazor WASM
    Debug.Assert(o.IsGlobalModeEnabled);
});

try
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");
    builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
    // Captures logError and higher as events
    builder.Logging.AddSentry(o => o.InitializeSdk = false);
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

    builder.Services.AddScoped<ErrorNotification>();

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

    builder.Services.AddScoped<IDataStore, DataStore>();
    builder.Services.AddSingleton<ErrorNotificationService>();

    var host = builder.Build();
    var logger = host.Services
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger<Program>();

    logger.LogInformation("App initialized.");
    logger.LogInformation($"Backend URI is set to {backendBaseAddress} and environment set to {builder.HostEnvironment.Environment}.");

    await host.RunAsync();
}
catch (Exception e)
{
    SentrySdk.CaptureException(e);
    await SentrySdk.FlushAsync();
    throw;
}