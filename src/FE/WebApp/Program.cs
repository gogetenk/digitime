using Digitime.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var backendBaseAddress = builder.Configuration.GetValue<string>("BackendBaseUri");
builder.Services.AddHttpClient(
        "Digitime.ServerAPI", 
        client => client.BaseAddress = new Uri(backendBaseAddress)
    )
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Digitime.ServerAPI"));
builder.Services.AddApiAuthorization();

var host = builder.Build();
var logger = host.Services.GetRequiredService<ILoggerFactory>()
    .CreateLogger<Program>();

logger.LogInformation("App initialized.");
logger.LogInformation($"Backend URI is set to {backendBaseAddress} and environment set to {builder.HostEnvironment.Environment}.");

await host.RunAsync();
