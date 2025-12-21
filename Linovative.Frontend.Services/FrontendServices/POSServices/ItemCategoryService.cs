using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.ItemDtos;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface IItemCategoryService : IReadOnlyService<ItemCategoryViewDto>, ICrudInterfaces
    {
        public Task<Response<ItemCategoryInputDto>> GetForUpdate(Guid id, CancellationToken token);
    }

    public class ItemCategoryService : CrudServiceAbstract<ItemCategoryViewDto>, IItemCategoryService
    {
        public ItemCategoryService(IHttpClientFactory httpFactory, ILogger<ItemCategoryService> logger) : base(httpFactory, logger, "ItemCategories")
        {
        }


        public async Task<Response<ItemCategoryInputDto>> GetForUpdate(Guid id, CancellationToken token) =>
            await base.GetForUpdateByID<ItemCategoryInputDto>(id, token);

    }
}
