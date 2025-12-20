using Linovative.Frontend.Services.Extensions;
using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.Outlets;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IOutletService : IReadOnlyService<OutletViewDto>, ICrudInterfaces
    {

    }

    public class OutletService : CrudServiceAbstract<OutletViewDto>, IOutletService
    {
        public OutletService (IHttpClientFactory httpFactory, ILogger<OutletService> logger) : base(httpFactory, logger, "Outlets")
        {
        }


        public override async Task<Response<OutletViewDto>> Get(Guid id, CancellationToken token, string? odataOption = null)
        {
            try
            {
                var url = $"{_uriPrefix}/{id}";
                var httpResponse = await _httpClient.GetAsync(url, token);
                var response = await httpResponse.ToAppResponse<OutletViewDto>(token);
                if (response) return response!;

                return Response<OutletViewDto>.Failed(response.Title, response.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<OutletViewDto>.Failed(Messages.GeneralErrorMessage);
            }
        }
    }
}
