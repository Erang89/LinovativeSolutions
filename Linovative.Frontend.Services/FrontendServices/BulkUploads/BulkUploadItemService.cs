using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.BulkUploads;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.BulkUploads
{
    public interface IBulkUploadItemService : IReadOnlyService<BulkUploadItemDto>, ICrudInterfaces
    {

    }

    public class BulkUploadItemService : CrudServiceAbstract<BulkUploadItemDto>, IBulkUploadItemService
    {
        public BulkUploadItemService(IHttpClientFactory httpFactory, ILogger<BulkUploadItemService> logger) : base(httpFactory, logger, "BulkUploadItems")
        {
        }

    }
}
