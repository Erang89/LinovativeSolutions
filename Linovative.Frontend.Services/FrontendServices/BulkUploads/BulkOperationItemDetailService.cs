using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.BulkUploads;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.BulkUploads
{
    public interface IBulkOperationItemDetailService : IReadOnlyService<BulkUploadItemDetailDto>, ICrudInterfaces
    {

    }

    public class BulkOperationItemDetailService : CrudServiceAbstract<BulkUploadItemDetailDto>, IBulkOperationItemDetailService
    {
        public BulkOperationItemDetailService(IHttpClientFactory httpFactory, ILogger<BulkOperationItemDetailService> logger) : base(httpFactory, logger, "BulkOperationItemDetails")
        {
        }
    }
}
