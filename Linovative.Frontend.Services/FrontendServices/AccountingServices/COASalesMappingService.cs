using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.MasterData.Accountings;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices.AccountingServices
{
    public interface ICOASalesMappingService : IReadOnlyService<SalesCOAMappingViewDto>, ICrudInterfaces
    {

    }

    public class CoaSalesMappingService : CrudServiceAbstract<SalesCOAMappingViewDto>, ICOASalesMappingService
    {
        public CoaSalesMappingService(IHttpClientFactory httpFactory, ILogger<CoaSalesMappingService> logger) : base(httpFactory, logger, "SalesCOAMappings")
        {
        }
    }
}
