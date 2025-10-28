using Linovative.Frontend.Services.Models;
using Linovative.Frontend.Shared.ShareServices;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Linovative.Frontent.WebBlazor.Extensions
{
    public static class EndpointExtensions
    {
        public static void ConfigureEndpoints(this WebAssemblyHostBuilder builder)
        {

            var appConfig = new ClientAppConfig();
            builder.Configuration.Bind("AppConfig", appConfig);
            builder.Services.AddHttpClient(EndpointNames.API, client =>
            {
                client.BaseAddress = new Uri(appConfig.ApiEndpoint ?? throw new NotImplementedException("Api Endpoint Not Configure"));
            }).AddHttpMessageHandler<HttpClientHeaderService>(); ;

            builder.Services.AddHttpClient(EndpointNames.RefreshAPI, client =>
            {
                client.BaseAddress = new Uri(appConfig.ApiEndpoint ?? throw new NotImplementedException("Api Endpoint Not Configure"));
            });

        }
    }
}
