using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.OrderTypes;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IOrderTypeService : IReadOnlyService<OrderTypeViewDto>, ICrudInterfaces
    {
        public Task<Response<OrderTypeInputDto>> GetForUpdate(Guid id, CancellationToken token);
    }

    public class OrderTypeService : CrudServiceAbstract<OrderTypeViewDto>, IOrderTypeService
    {
        public OrderTypeService(IHttpClientFactory httpFactory, ILogger<OrderTypeService> logger) : base(httpFactory, logger, "OrderTypes")
        {
        }

        public async Task<Response<OrderTypeInputDto>> GetForUpdate(Guid id, CancellationToken token) =>
            await base.GetForUpdateByID<OrderTypeInputDto>(id, token);
    }
}
