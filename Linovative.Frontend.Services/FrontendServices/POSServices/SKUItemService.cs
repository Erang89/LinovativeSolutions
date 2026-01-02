using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.ItemDtos;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface ISKUItemService : IReadOnlyService<SKUItemViewDto>, ICrudInterfaces
    {
    }

    public class SKUItemService : CrudServiceAbstract<SKUItemViewDto>, ISKUItemService
    {
        public SKUItemService(IHttpClientFactory httpFactory, ILogger<SKUItemService> logger) : base(httpFactory, logger, "SKUItems")
        {
        }

       
    }
}
