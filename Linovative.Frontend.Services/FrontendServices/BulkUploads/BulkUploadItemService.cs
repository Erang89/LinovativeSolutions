using Linovative.Frontend.Services.Extensions;
using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.BulkUploads;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.BulkUploads
{
    public interface IBulkUploadItemService : IReadOnlyService<BulkUploadItemDto>, ICrudInterfaces
    {
        public Task<Response> RemoveBulkCreate(CancellationToken token);
        public Task<Response> RemoveBulkUpdate(CancellationToken token);
        public Task<Response> RemoveBulkDelete(CancellationToken token);
    }

    public class BulkUploadItemService : CrudServiceAbstract<BulkUploadItemDto>, IBulkUploadItemService
    {
        public BulkUploadItemService(IHttpClientFactory httpFactory, ILogger<BulkUploadItemService> logger) : base(httpFactory, logger, "BulkUploadItems")
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

        public Task<Response> RemoveBulkUpdate(CancellationToken token) => Delete("BulkUpdate", token);

        public Task<Response> RemoveBulkDelete(CancellationToken token) => Delete("BulkDelete", token);


    }
}
