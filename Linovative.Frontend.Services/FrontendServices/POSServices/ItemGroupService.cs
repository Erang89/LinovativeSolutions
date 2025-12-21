using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.ItemDtos;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IItemGroupService : IReadOnlyService<ItemGroupViewDto>, ICrudInterfaces
    {
        public Task<Response<ItemGroupInputDto>> GetForUpdate(Guid id, CancellationToken token);
    }

    public class ItemGroupService : CrudServiceAbstract<ItemGroupViewDto>, IItemGroupService
    {
        public ItemGroupService(IHttpClientFactory httpFactory, ILogger<ItemGroupService> logger) : base(httpFactory, logger, "ItemGroups")
        {
        }


        public async Task<Response<ItemGroupInputDto>> GetForUpdate(Guid id, CancellationToken token) =>
            await base.GetForUpdateByID<ItemGroupInputDto>(id, token);
    }
}
