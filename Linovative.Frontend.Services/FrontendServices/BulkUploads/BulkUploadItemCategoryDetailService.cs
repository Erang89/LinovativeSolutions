using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.BulkUploads;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.BulkUploads
{
    public interface IBulkUploadItemCategoryDetailService : IReadOnlyService<BulkUploadItemCategoryDetailDto>, ICrudInterfaces
    {

    }

    public class BulkUploadItemCategoryDetailService : CrudServiceAbstract<BulkUploadItemCategoryDetailDto>, IBulkUploadItemCategoryDetailService
    {
        public BulkUploadItemCategoryDetailService(IHttpClientFactory httpFactory, ILogger<BulkUploadItemCategoryDetailService> logger) : base(httpFactory, logger, "BulkUploadItemCategoryDetails")
        {
        }
    }
}
