using Linovative.Frontend.Services.Constans;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using Blazored.SessionStorage;
using Linovative.Frontend.Services.Extensions;

namespace Linovative.Frontend.Shared.ShareServices
{
    internal class JwtTokenService : IJwtTokenProvider
    {
        private readonly IStorage _storage;
        private readonly ILogger _logger;
        private const string RefreshUrl = "Auth/RefreshToken";
        private readonly ISessionStorageService _session;
        private readonly IUnauthorizeHandler _unauthorizeHandler;

        public JwtTokenService(IStorage storage, ILogger<JwtTokenService> logger, ISessionStorageService session, IUnauthorizeHandler unauthorizeHandler)
        {
            _storage = storage;
            _logger = logger;
            _session = session;
            _unauthorizeHandler = unauthorizeHandler;
        }

        internal class RefreshTokenInput
        {
            public string? RefreshToken { get; set; }
        }

        public async Task<JwtToken?> GetJwtToken(HttpClient? httpClient = default, CancellationToken token = default)
        {
            try
            {
                var jwt = await GetJwtTokenFromSession(token);
                var isSessionExist = jwt is not null;
                var jwtFromStorage = await _storage.GetValue<JwtToken>(StorageKeys.Token);
                jwt = jwt is null ? jwtFromStorage : jwt;
                if (jwt == null || jwtFromStorage == null || (isSessionExist && !jwt.UIDHash!.Equals(jwtFromStorage.UIDHash))) return null;

                var utcTime = DateTime.UtcNow;
                if (jwt.ExpireAtUtcTime <= utcTime)
                {
                    if (httpClient is null || jwt.RefreshToken is null) return null;
                    var input = new RefreshTokenInput() { RefreshToken = jwt.RefreshToken };
                    var httpResponse = await httpClient.PostAsJsonAsync(RefreshUrl, input, token);
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await _unauthorizeHandler.Handle(token);
                        return null;
                    }
                    var response = await httpResponse.ToAppResponse<JwtToken>(token);
                    if (!response) return null;
                    await ChangeToken(response!.Data!, token);
                    jwt = response.Data!;
                }

                if (!isSessionExist) await _session.SetItemAsync<JwtToken>(StorageKeys.Token, jwt);

                return jwt;
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

        public async Task ChangeToken(JwtToken jwtToken, CancellationToken token = default)
        {
            await _storage.SetValue(StorageKeys.Token, jwtToken);
            await _session.SetItemAsync<JwtToken>(StorageKeys.Token, jwtToken, token);
        }
    }
}
