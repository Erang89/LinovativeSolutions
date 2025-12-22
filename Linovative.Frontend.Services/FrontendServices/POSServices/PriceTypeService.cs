using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.ItemDtos;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IPriceTypeService : IReadOnlyService<PriceTypeDto>, ICrudInterfaces
    {

    }

    public class PriceTypeService : CrudServiceAbstract<PriceTypeDto>, IPriceTypeService
    {
        public PriceTypeService(IHttpClientFactory httpFactory, ILogger<PriceTypeService> logger) : base(httpFactory, logger, "PriceTypes")
        {
        }
    }
}
