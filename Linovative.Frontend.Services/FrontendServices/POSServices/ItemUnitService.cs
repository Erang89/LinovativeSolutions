using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.ItemDtos;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IItemUnitService : IReadOnlyService<ItemUnitDto>, ICrudInterfaces
    {

    }

    public class ItemUnitService : CrudServiceAbstract<ItemUnitDto>, IItemUnitService
    {
        public ItemUnitService(IHttpClientFactory httpFactory, ILogger<ItemUnitService> logger) : base(httpFactory, logger, "ItemUnits")
        {
        }

    }
}
