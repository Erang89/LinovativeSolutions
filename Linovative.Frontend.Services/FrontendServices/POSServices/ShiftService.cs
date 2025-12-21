using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.MasterData.Shifts;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IShiftService : IReadOnlyService<ShiftViewDto>, ICrudInterfaces
    {
        public Task<Response<ShiftUpdateDto>> GetForUpdate(Guid id, CancellationToken token);
    }

    public class ShiftService : CrudServiceAbstract<ShiftViewDto>, IShiftService
    {
        public ShiftService(IHttpClientFactory httpFactory, ILogger<ShiftService> logger) : base(httpFactory, logger, "Shifts")
        {
        }

        public async Task<Response<ShiftUpdateDto>> GetForUpdate(Guid id, CancellationToken token) =>
            await base.GetForUpdateByID<ShiftUpdateDto>(id, token);
    }
}
