using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.BulkUploads;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.BulkUploads
{
    public interface IBulkOperationItemUnitDetailService : IReadOnlyService<BulkUploadItemUnitDetailDto>, ICrudInterfaces
    {

    }

    public class BulkOperationItemUnitDetailService : CrudServiceAbstract<BulkUploadItemUnitDetailDto>, IBulkOperationItemUnitDetailService
    {
        public BulkOperationItemUnitDetailService(IHttpClientFactory httpFactory, ILogger<BulkOperationItemUnitDetailService> logger) : base(httpFactory, logger, "BulkOperationItemUnitDetails")
        {
        }
    }
}
