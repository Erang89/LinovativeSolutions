using Linovative.Frontend.Services.Constans;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using Blazored.SessionStorage;
using Linovative.Frontend.Services.Extensions;

namespace Linovative.Frontend.Shared.ShareServices
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IStorageService _storage;
        private readonly ILogger _logger;
        private const string RefreshUrl = "Auth/Jwt/Refresh";
        private readonly ISessionStorageService _session;
        private readonly IUnauthorizeHandlerService _unauthorizeHandler;

        public JwtTokenService(IStorageService storage, ILogger<JwtTokenService> logger, ISessionStorageService session, IUnauthorizeHandlerService unauthorizeHandler)
        {
            _storage = storage;
            _logger = logger;
            _session = session;
            _unauthorizeHandler = unauthorizeHandler;
        }


        public async Task<JwtToken?> GetJwtToken(HttpClient? httpClient = default, CancellationToken cancelToken = default)
        {
            try
            {
                var jwTokenFromSession = await GetJwtTokenFromSession(cancelToken);
                var jwtFromStorage = await _storage.GetValue<JwtToken>(StorageKeys.Token);
                var jwtToken = jwTokenFromSession?? jwtFromStorage;
                if (jwtToken is null) return null;

                var utcTime = DateTime.UtcNow;
                if (jwtToken.ExpireAtUtcTime <= utcTime)
                {
                    if (httpClient is null) return null;

                    var input = new { token = jwtToken.RefreshToken, companyId = jwtToken.CompanyId };
                    var httpResponse = await httpClient.PostAsJsonAsync(RefreshUrl, input, cancelToken);
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await _unauthorizeHandler.Handle(cancelToken);
                        return null;
                    }
                    var response = await httpResponse.ToAppResponse<JwtToken>(cancelToken);
                    if (!response) return null;
                    await SetToken(response!.Data!, cancelToken);
                    jwtToken = response.Data!;
                }

                return jwtToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accurred when trying to get JWT Token");
                return null;
            }
        }

        private async Task<JwtToken?> GetJwtTokenFromSession(CancellationToken token = default)
        {
            try
            {
                var jwt = await _session.GetItemAsync<JwtToken>(StorageKeys.Token);
                if (jwt == null) return null;
                return jwt;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accurred when trying to get JWT Token");
                return null;
            }
        }

        public async Task SetToken(JwtToken jwtToken, CancellationToken token = default)
        {
            await _storage.SetValue(StorageKeys.Token, jwtToken);
            await _session.SetItemAsync<JwtToken>(StorageKeys.Token, jwtToken, token);
        }
    }
}
