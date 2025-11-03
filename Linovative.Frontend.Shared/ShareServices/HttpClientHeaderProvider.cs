using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Net;

namespace Linovative.Frontend.Shared.ShareServices
{
    public class HttpClientHeaderProvider : DelegatingHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUnauthorizeHandlerService _unauthorizeHandler;

        public HttpClientHeaderProvider(IServiceProvider serviceProvider, IUnauthorizeHandlerService unauthorizeHandler)
        {
            _serviceProvider = serviceProvider;
            _unauthorizeHandler = unauthorizeHandler;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Dynamically resolve the IJwtTokenProvider
            using var scope = _serviceProvider.CreateScope();
            var jwtTokenProvider = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();
            var httpFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpFactory.CreateClient(EndpointNames.PublicApi);
            var jwtToken = await jwtTokenProvider.GetJwtToken(httpClient);

            if (jwtToken == null)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            // Clear and set headers for each request
            request.Headers.Clear();
            request.Headers.Authorization = new AuthenticationHeaderValue(HttpHeaderKeys.Bearer, jwtToken!.Token);


            // Continue the request pipeline
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _unauthorizeHandler.Handle(cancellationToken);
            }

            return response;
        }

    }
}
