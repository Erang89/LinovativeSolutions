using Linovative.Frontend.Services.Constans;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.Auth;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Blazored.SessionStorage;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IAuthenticationService
    {
        public Task<Response<JwtToken>> AssignCompanyId(Guid companyId, CancellationToken cancellationToken = default);
        public Task<Response<JwtToken>> IsAuthenticated(CancellationToken token);
        public Task<Response> Login(LoginDto dto, CancellationToken token);
        public Task Logout();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClientRefresh;
        private readonly IStorageService _storage;
        private readonly ILogger _logger;
        private const string LoginUrl = "auth/login";
        private const string AssignCompanyIdUrl = "auth/AssignCompanyId";
        private readonly ISessionStorageService _session;
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public AuthenticationService(
            IStorageService storage,
            IHttpClientFactory httpFactory,
            ILogger<AuthenticationService> logger,
            ISessionStorageService session,
            IJwtTokenProvider jwtTokenProvider
            )
        {
            _storage = storage;
            _httpClient = httpFactory.CreateClient(EndpointNames.PrivateApi);
            _httpClientRefresh = httpFactory.CreateClient(EndpointNames.PrivateApi);
            _logger = logger;
            _session = session;
            _jwtTokenProvider = jwtTokenProvider;
        }
        public async Task<Response<JwtToken>> IsAuthenticated(CancellationToken token)
        {
            try
            {
                var jwt = await _jwtTokenProvider.GetJwtToken(_httpClientRefresh, token);
                if (jwt == null) return Response<JwtToken>.Failed("Not authenticated");

                return Response<JwtToken>.Ok(jwt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failled when authenticate");
                return Response<JwtToken>.Failed("Failed Authenticate");
            }
        }



        public async Task<Response> Login(LoginDto dto, CancellationToken token)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(LoginUrl, dto, token);
                if (!response.IsSuccessStatusCode)
                    return Response.Failed("Invalid user name or password");

                var jsonString = await response.Content.ReadAsStringAsync(token);
                JwtToken? jwtInfo = (JsonConvert.DeserializeObject<Response<JwtToken>?>(jsonString))!.Data;

                await _storage.SetValue(StorageKeys.Token, jwtInfo!);

                return Response.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.Failed("An error accurred. Please contact the administrator");
            }
        }

        public async Task Logout()
        {
            await _storage.Remove(StorageKeys.Token);
            await _session.RemoveItemAsync(StorageKeys.Token);
        }

        public async Task<Response<JwtToken>> AssignCompanyId(Guid companyId, CancellationToken token = default)
        {
            try
            {
                var dto = new { CompanyId = companyId };
                var response = await _httpClient.PostAsJsonAsync(AssignCompanyIdUrl, dto, token);
                if (!response.IsSuccessStatusCode)
                    return Response<JwtToken>.Failed("Invalid user name or password");

                var jsonString = await response.Content.ReadAsStringAsync(token);
                JwtToken? jwtInfo = (JsonConvert.DeserializeObject<Response<JwtToken>?>(jsonString))!.Data;

                await _jwtTokenProvider.SetToken(jwtInfo!, token);

                return Response<JwtToken>.Ok(jwtInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<JwtToken>.Failed("An error accurred. Please contact the administrator");
            }
        }
    }
}
