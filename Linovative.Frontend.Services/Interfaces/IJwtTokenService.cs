using Linovative.Frontend.Services.Models;

namespace Linovative.Frontend.Services.Interfaces
{
    public interface IJwtTokenService
    {
        Task<JwtToken?> GetJwtToken(HttpClient? httpClient = default, CancellationToken token = default);
        Task SetToken(JwtToken jwtToken, CancellationToken token = default);
    }
}
