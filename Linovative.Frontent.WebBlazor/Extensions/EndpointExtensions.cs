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
            builder.Services.AddHttpClient(EndpointNames.PrivateApi, client =>
            {
                client.BaseAddress = new Uri(appConfig.PrivateEndpoint ?? throw new NotImplementedException("Api Endpoint Not Configure"));
            }).AddHttpMessageHandler<HttpClientHeaderService>(); ;

            builder.Services.AddHttpClient(EndpointNames.PublicApi, client =>
            {
                client.BaseAddress = new Uri(appConfig.PrivateEndpoint ?? throw new NotImplementedException("Api Endpoint Not Configure"));
            });

        }
    }
}
