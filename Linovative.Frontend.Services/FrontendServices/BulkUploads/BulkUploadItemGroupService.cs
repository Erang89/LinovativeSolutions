using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using LinoVative.Shared.Dto.BulkUploads;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.BulkUploads
{
    public interface IBulkUploadItemGroupService : IReadOnlyService<BulkUploadItemGroupDto>, ICrudInterfaces
    {

    }

    public class BulkUploadItemGroupService : CrudServiceAbstract<BulkUploadItemGroupDto>, IBulkUploadItemGroupService
    {
        public BulkUploadItemGroupService(IHttpClientFactory httpFactory, ILogger<BulkUploadItemGroupService> logger) : base(httpFactory, logger, "BulkUploadItemGroups")
        {
        }

    }
}
