using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.BulkUploads;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.BulkUploads
{
    public interface IBulkUploadItemGroupDetailService : IReadOnlyService<BulkUploadItemGroupDetailDto>, ICrudInterfaces
    {

    }

    public class BulkUploadItemGroupDetailService : CrudServiceAbstract<BulkUploadItemGroupDetailDto>, IBulkUploadItemGroupDetailService
    {
        public BulkUploadItemGroupDetailService(IHttpClientFactory httpFactory, ILogger<BulkUploadItemGroupDetailService> logger) : base(httpFactory, logger, "BulkUploadItemGroupDetails")
        {
        }
    }
}
