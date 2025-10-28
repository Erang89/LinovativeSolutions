using Linovative.Frontend.Services.Models;

namespace Linovative.Frontend.Services.Interfaces
{
    public interface IJwtTokenProvider
    {
        Task<JwtToken?> GetJwtToken(HttpClient? httpClient = default, CancellationToken token = default);
        Task ChangeToken(JwtToken jwtToken, CancellationToken token = default);
    }
}
