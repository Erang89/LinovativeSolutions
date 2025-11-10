using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.Outlets;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IOutletAreaService : IReadOnlyService<OutletAreaViewDto>, ICrudInterfaces
    {

    }

    public class OutletAreaService : CrudServiceAbstract<OutletAreaViewDto>, IOutletAreaService
    {
        public OutletAreaService(IHttpClientFactory httpFactory, ILogger<OutletAreaService> logger) : base(httpFactory, logger, "OutletAreas")
        {
        }

    }
}
