using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.BulkUploads;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.BulkUploads
{
    public interface IBulkUploadItemCategoryService : IReadOnlyService<BulkUploadItemCategoryDto>, ICrudInterfaces
    {

    }

    public class BulkUploadItemCategoryService : CrudServiceAbstract<BulkUploadItemCategoryDto>, IBulkUploadItemCategoryService
    {
        public BulkUploadItemCategoryService(IHttpClientFactory httpFactory, ILogger<BulkUploadItemCategoryService> logger) : base(httpFactory, logger, "BulkUploadItemCategories")
        {
        }

    }
}
