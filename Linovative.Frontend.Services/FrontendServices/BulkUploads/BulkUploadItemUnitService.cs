using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.BulkUploads;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.BulkUploads
{
    public interface IBulkUploadItemUnitService : IReadOnlyService<BulkUploadItemUnitDto>, ICrudInterfaces
    {

    }

    public class BulkUploadItemUnitService : CrudServiceAbstract<BulkUploadItemUnitDto>, IBulkUploadItemUnitService
    {
        public BulkUploadItemUnitService(IHttpClientFactory httpFactory, ILogger<BulkUploadItemUnitService> logger) : base(httpFactory, logger, "BulkUploadItemUnits")
        {
        }

    }
}
