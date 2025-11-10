using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.MasterData.Shifts;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IShiftService : IReadOnlyService<ShiftViewDto>, ICrudInterfaces
    {

    }

    public class ShiftService : CrudServiceAbstract<ShiftViewDto>, IShiftService
    {
        public ShiftService(IHttpClientFactory httpFactory, ILogger<ShiftService> logger) : base(httpFactory, logger, "Shifts")
        {
        }

    }
}
