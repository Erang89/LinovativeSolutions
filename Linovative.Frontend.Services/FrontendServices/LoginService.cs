using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Models;
using Microsoft.Extensions.Logging;
using Linovative.Frontend.Services.Extensions;
using LinoVative.Shared.Dto.Auth;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface ILoginService
    {
        public Task<Response> Login(LoginDto dto, CancellationToken token);
    }

    public class LoginService : RequeserServiceBase, ILoginService
    {
        public LoginService(IHttpClientFactory httpFactory, ILogger<RegisterNewCompanyService> logger) : base(httpFactory, logger, "auth")
        {
            
        }
        protected override bool IsPublicEndpoint => true;

        public async Task<Response> Login(LoginDto dto, CancellationToken token)
        {
            try
            {
                var response = await base.Post("login", dto, token);
                return await response.ToAppBoolResponse(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.Failed(Messages.GeneralErrorMessage);
            }

        }
    }
}
