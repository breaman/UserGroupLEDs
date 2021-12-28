using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UserGroupLEDs.Web;
using UserGroupLEDs.Web.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient<WeatherClient>(client =>
    {
         //client.BaseAddress = builder.Configuration.GetServiceUri("usergroupleds-api");
         //builder.Configuration.Bind("apiHost", )
         Console.WriteLine(builder.Configuration["apiHost"]);
         client.BaseAddress = new Uri(builder.Configuration["apiHost"]);
    });

Console.WriteLine("Stokes: just a test");

await builder.Build().RunAsync();
