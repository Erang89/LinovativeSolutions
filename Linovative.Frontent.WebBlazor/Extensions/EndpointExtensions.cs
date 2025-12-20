using Linovative.Frontend.Services.Converter;
using Linovative.Frontend.Services.Models;
using Linovative.Frontend.Shared.ShareServices;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Text.Json;

namespace Linovative.Frontent.WebBlazor.Extensions
{
    public static class EndpointExtensions
    {
        public static void ConfigureEndpoints(this WebAssemblyHostBuilder builder)
        {

            var appConfig = builder.Configuration
                                  .GetSection("AppConfig")
                                  .Get<ClientAppConfig>() ?? new ClientAppConfig();

            builder.Services.AddHttpClient(EndpointNames.PrivateApi, client =>
            {
                client.BaseAddress = new Uri(appConfig.PrivateEndpoint ?? throw new NotImplementedException("Api Endpoint Not Configure"));
            })
            .AddHttpMessageHandler<HttpClientHeaderProvider>(); // requires HttpClientHeaderProvider registered

            builder.Services.AddHttpClient(EndpointNames.PublicApi, client =>
            {
                client.BaseAddress = new Uri(appConfig.PublicEndpoint ?? throw new NotImplementedException("Public Api Endpoint Not Configure"));
            });

            builder.Services.Configure<JsonSerializerOptions>(options =>
            {
                options.Converters.Add(new Iso8601TimeSpanConverter());
            });

        }
    }
}
