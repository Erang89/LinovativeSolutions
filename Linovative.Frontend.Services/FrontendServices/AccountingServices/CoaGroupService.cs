using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.MasterData.Accountings;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices.AccountingServices
{
    public interface ICOAGroupService : IReadOnlyService<COAGroupDto>, ICrudInterfaces
    {

    }

    public class CoaGroupService : CrudServiceAbstract<COAGroupDto>, ICOAGroupService
    {
        public CoaGroupService(IHttpClientFactory httpFactory, ILogger<CoaGroupService> logger) : base(httpFactory, logger, "COAGroups")
        {
        }

    }
}
