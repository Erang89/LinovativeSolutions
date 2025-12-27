using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.ItemDtos;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IItemService : IReadOnlyService<ItemViewDto>, ICrudInterfaces
    {
        public Task<Response<ItemInputDto>> GetForUpdate(Guid id, CancellationToken token);
    }

    public class ItemService : CrudServiceAbstract<ItemViewDto>, IItemService
    {
        public ItemService(IHttpClientFactory httpFactory, ILogger<ItemService> logger) : base(httpFactory, logger, "Items")
        {
        }

        public async Task<Response<ItemInputDto>> GetForUpdate(Guid id, CancellationToken token) =>
            await base.GetForUpdateByID<ItemInputDto>(id, token);

    }
}
