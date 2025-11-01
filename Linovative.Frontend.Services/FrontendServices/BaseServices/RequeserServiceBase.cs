using Linovative.Frontend.Services.Extensions;
using Linovative.Frontend.Services.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Linovative.Frontend.Services.FrontendServices.BaseServices
{
    public abstract class RequeserServiceBase
    {

        protected HttpClient _httpClient => IsPublicEndpoint ? _publicHttpClient : _privateHttpClient ?? new HttpClient();
        protected readonly HttpClient _privateHttpClient;
        protected readonly HttpClient _publicHttpClient;
        protected readonly ILogger _logger;

        protected const string CREATE = "create";
        protected const string UPDATE = "update";
        protected const string DELETE = "delete";

        protected readonly string _uriPrefix;

        protected abstract bool IsPublicEndpoint { get; }

        protected RequeserServiceBase(IHttpClientFactory httpFactory, ILogger logger, string uriPrefix)
        {
            _logger = logger;
            _privateHttpClient = httpFactory.CreateClient(EndpointNames.PrivateApi);
            _publicHttpClient = httpFactory.CreateClient(EndpointNames.PublicApi);
            _uriPrefix = uriPrefix;
        }


        public virtual async Task<HttpResponseMessage> Post(string url, object obj, CancellationToken token)
        {
            return await _httpClient.PostAsJsonAsync($"{_uriPrefix}/{url}", obj, token);
        }

    }
}
