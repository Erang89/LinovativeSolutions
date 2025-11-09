using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.OrderTypes;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IOrderTypeService : IReadOnlyService<OrderTypeViewDto>, ICrudInterfaces
    {

    }

    public class OrderTypeService : CrudServiceAbstract<OrderTypeViewDto>, IOrderTypeService
    {
        public OrderTypeService(IHttpClientFactory httpFactory, ILogger<OrderTypeService> logger) : base(httpFactory, logger, "OrderTypes")
        {
        }

    }
}
