using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.ItemDtos;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface ICostumePriceTagService : IReadOnlyService<CostumePriceTagDto>, ICrudInterfaces
    {

    }

    public class CostumePriceTagService : CrudServiceAbstract<CostumePriceTagDto>, ICostumePriceTagService
    {
        public CostumePriceTagService(IHttpClientFactory httpFactory, ILogger<CostumePriceTagService> logger) : base(httpFactory, logger, "CostumePriceTags")
        {
        }
    }
}
