using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.BulkUploads;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.BulkUploads
{
    public interface IBulkOperationItemCategoryDetailService : IReadOnlyService<BulkUploadItemCategoryDetailDto>, ICrudInterfaces
    {

    }

    public class BulkOperationItemCategoryDetailService : CrudServiceAbstract<BulkUploadItemCategoryDetailDto>, IBulkOperationItemCategoryDetailService
    {
        public BulkOperationItemCategoryDetailService(IHttpClientFactory httpFactory, ILogger<BulkOperationItemCategoryDetailService> logger) : base(httpFactory, logger, "BulkOperationItemCategoryDetails")
        {
        }
    }
}
