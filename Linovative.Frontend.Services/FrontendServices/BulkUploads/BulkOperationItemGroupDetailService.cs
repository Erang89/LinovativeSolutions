using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.BulkUploads;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.BulkUploads
{
    public interface IBulkOperationItemGroupDetailService : IReadOnlyService<BulkUploadItemGroupDetailDto>, ICrudInterfaces
    {

    }

    public class BulkOperationItemGroupDetailService : CrudServiceAbstract<BulkUploadItemGroupDetailDto>, IBulkOperationItemGroupDetailService
    {
        public BulkOperationItemGroupDetailService(IHttpClientFactory httpFactory, ILogger<BulkOperationItemGroupDetailService> logger) : base(httpFactory, logger, "BulkOperationItemGroupDetails")
        {
        }
    }
}
