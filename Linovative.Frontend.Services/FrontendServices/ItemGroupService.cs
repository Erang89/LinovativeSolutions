using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.ItemDtos;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IItemGroupService : IReadOnlyService<ItemGroupViewDto>, ICrudInterfaces
    {

    }

    public class ItemGroupService : CrudServiceAbstract<ItemGroupViewDto>, IItemGroupService
    {
        public ItemGroupService(IHttpClientFactory httpFactory, ILogger<ItemGroupService> logger) : base(httpFactory, logger, "ItemCategories")
        {
        }

        protected override bool IsPublicEndpoint => false;
    }
}
