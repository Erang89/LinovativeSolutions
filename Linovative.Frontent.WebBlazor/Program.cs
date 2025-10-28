using Blazored.LocalStorage;
using Linovative.Frontent.WebBlazor;
using Linovative.Frontent.WebBlazor.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.ConfigureEndpoints();
builder.Services.AddLocalization();
builder.Services.AddBlazoredLocalStorage();
builder.RegisterAllServices();
await builder.RegisterLocalizer();

await builder.Build().RunAsync();
