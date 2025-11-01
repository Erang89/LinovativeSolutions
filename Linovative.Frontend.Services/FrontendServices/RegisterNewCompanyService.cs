using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using Linovative.Frontend.Services.Extensions;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IRegisterCompanyService
    {
        public Task<Response> Register(RegisterNewCompanyDto dto, CancellationToken token);
    }

    public class RegisterNewCompanyService : RequeserServiceBase, IRegisterCompanyService
    {
        public RegisterNewCompanyService(IHttpClientFactory httpFactory, ILogger<RegisterNewCompanyService> logger) : base(httpFactory, logger, "companies")
        {
            
        }
        protected override bool IsPublicEndpoint => true;

        public async Task<Response> Register(RegisterNewCompanyDto dto, CancellationToken token)
        {
            try
            {
                var response = await base.Post("register", dto, token);
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
