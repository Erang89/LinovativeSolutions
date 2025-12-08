using Linovative.Frontend.Services.Extensions;
using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.BulkUploads;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.BulkUploads
{
    public interface IBulkOperationItemGroupService : IReadOnlyService<BulkUploadItemGroupDto>, ICrudInterfaces
    {
        public Task<Response> RemoveBulkCreate(CancellationToken token);
        public Task<Response> RemoveBulkUpdate(CancellationToken token);
        public Task<Response> RemoveBulkDelete(CancellationToken token);

    }

    public class BulkOperationItemGroupService : CrudServiceAbstract<BulkUploadItemGroupDto>, IBulkOperationItemGroupService
    {
        public BulkOperationItemGroupService(IHttpClientFactory httpFactory, ILogger<BulkOperationItemGroupService> logger) : base(httpFactory, logger, "BulkOperationItemGroups")
        {
        }


        private async Task<Response> Delete(string lastUri, CancellationToken token)
        {
            try
            {
                var httpResponse = await _httpClient.DeleteAsync($"{_uriPrefix}/remove/{lastUri}", token);
                var response = await httpResponse.ToAppBoolResponse(token);
                if (response)
                    return response;

                return Response.Failed(response.Title, response.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.Failed(Messages.GeneralErrorMessage);
            }
        }

        public Task<Response> RemoveBulkCreate(CancellationToken token) => Delete("BulkCreate", token);

        public Task<Response> RemoveBulkDelete(CancellationToken token) => Delete("BulkUpdate", token);

        public Task<Response> RemoveBulkUpdate(CancellationToken token) => Delete("BulkDelete", token);
    }
}
