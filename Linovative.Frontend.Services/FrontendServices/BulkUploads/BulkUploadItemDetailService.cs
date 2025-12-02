using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.BulkUploads;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.BulkUploads
{
    public interface IBulkUploadItemDetailService : IReadOnlyService<BulkUploadItemDetailDto>, ICrudInterfaces
    {

    }

    public class BulkUploadItemDetailService : CrudServiceAbstract<BulkUploadItemDetailDto>, IBulkUploadItemDetailService
    {
        public BulkUploadItemDetailService(IHttpClientFactory httpFactory, ILogger<BulkUploadItemDetailService> logger) : base(httpFactory, logger, "BulkUploadItemDetails")
        {
        }
    }
}
