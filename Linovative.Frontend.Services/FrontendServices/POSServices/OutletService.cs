using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
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

    }
}
