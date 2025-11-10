using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.ItemDtos;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IItemService : IReadOnlyService<ItemViewDto>, ICrudInterfaces
    {

    }

    public class ItemService : CrudServiceAbstract<ItemViewDto>, IItemService
    {
        public ItemService(IHttpClientFactory httpFactory, ILogger<ItemService> logger) : base(httpFactory, logger, "Items")
        {
        }

    }
}
