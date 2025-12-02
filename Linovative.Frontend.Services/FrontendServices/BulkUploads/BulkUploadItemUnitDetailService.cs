using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.BulkUploads;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.BulkUploads
{
    public interface IBulkUploadItemUnitDetailService : IReadOnlyService<BulkUploadItemUnitDetailDto>, ICrudInterfaces
    {

    }

    public class BulkUploadItemUnitDetailService : CrudServiceAbstract<BulkUploadItemUnitDetailDto>, IBulkUploadItemUnitDetailService
    {
        public BulkUploadItemUnitDetailService(IHttpClientFactory httpFactory, ILogger<BulkUploadItemUnitDetailService> logger) : base(httpFactory, logger, "BulkUploadItemUnitDetails")
        {
        }
    }
}
