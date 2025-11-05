using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.ItemDtos;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IItemCategoryService : IReadOnlyService<ItemCategoryViewDto>, ICrudInterfaces
    {

    }

    public class ItemCategoryService : CrudServiceAbstract<ItemCategoryViewDto>, IItemCategoryService
    {
        public ItemCategoryService(IHttpClientFactory httpFactory, ILogger<ItemCategoryService> logger) : base(httpFactory, logger, "ItemCategories")
        {
        }

    }
}
