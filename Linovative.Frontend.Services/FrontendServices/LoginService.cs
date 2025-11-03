using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Models;
using Microsoft.Extensions.Logging;
using Linovative.Frontend.Services.Extensions;
using LinoVative.Shared.Dto.Auth;
using Linovative.Frontend.Services.Interfaces;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface ILoginService
    {
        public Task<Response<JwtToken>> Login(LoginDto dto, CancellationToken token);
    }

    public class LoginService : RequeserServiceBase, ILoginService
    {
        private readonly IJwtTokenService _jwtService;
        public LoginService(IHttpClientFactory httpFactory, ILogger<RegisterNewCompanyService> logger, IJwtTokenService storageService) : base(httpFactory, logger, "auth")
        {
            _jwtService = storageService;
        }
        protected override bool IsPublicEndpoint => true;

        public async Task<Response<JwtToken>> Login(LoginDto dto, CancellationToken token)
        {
            try
            {
                var httpResponse = await base.Post("login", dto, token);
                var response = await httpResponse.ToAppResponse<JwtToken>(token);

                if (response)
                {
                    await _jwtService.SetToken(response.Data!);
                }
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<JwtToken>.Failed(Messages.GeneralErrorMessage);
            }

        }
    }
}
