using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Linovative.Frontend.Services.Configuration;
using Linovative.Frontend.Services.Converter;
using Linovative.Frontend.Shared.ShareServices;
using Linovative.Frontent.WebBlazor;
using Linovative.Frontent.WebBlazor.Extensions;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();
builder.Services.AddLogging();
builder.Services.AddScoped<HttpClientHeaderProvider>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.ConfigureEndpoints();
var jsonOptions = new System.Text.Json.JsonSerializerOptions
{
    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
};
jsonOptions.Converters.Add(new Iso8601TimeSpanConverter()); // System.Text.Json converter

builder.Services.AddSingleton(jsonOptions);


builder.Services.AddLocalization();
builder.Services.AddBlazoredLocalStorage(config =>
    config.JsonSerializerOptions.WriteIndented = true);
builder.Services.AddBlazoredSessionStorage(config =>
    config.JsonSerializerOptions.WriteIndented = true);

TypeAdapterConfig.GlobalSettings.Scan(typeof(MapsterConfigs).Assembly);
builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
builder.Services.AddScoped<IMapper, ServiceMapper>();

builder.RegisterAllServices();
await builder.RegisterLocalizer();

await builder.Build().RunAsync();
