using Linovative.Frontend.Services.Extensions;
using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.BulkUploads
{
    public interface IBulkUploadService 
    {
        public Task<Response<Guid?>> UploadItemGroups(IBrowserFile file, CancellationToken token);
        public Task<Response<Guid?>> UploadItemCategories(IBrowserFile file, CancellationToken token);
        public Task<Response<Guid?>> UploadItemUnits(IBrowserFile file, CancellationToken token);
        public Task<Response<Guid?>> UploadItems(IBrowserFile file, CancellationToken token);
    }

    public class BulkUploadService : RequeserServiceBase, IBulkUploadService
    {

        private readonly long maxFileSize = 20 * 1024 * 1024; // 20 MB

        public BulkUploadService(IHttpClientFactory httpFactory, ILogger<BulkUploadService> logger) : base(httpFactory, logger, "BulkUploads")
        {
        }

        private async Task<Response<Guid?>> UploadFile(IBrowserFile file, string endpoint, CancellationToken token)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                using var stream = file!.OpenReadStream(maxFileSize);
                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType ?? "application/octet-stream");
                content.Add(streamContent, "file", file.Name);
                var httpResponse = await _httpClient.PostAsync($"{_uriPrefix}/{endpoint}", content, token);
                var response = await httpResponse.ToAppResponse<Guid?>(token);
                if (response)
                    return response;

                return Response<Guid?>.Failed(response.Title, response.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<Guid?>.Failed(Messages.GeneralErrorMessage);
            }
        }

        public async Task<Response<Guid?>> UploadItemGroups(IBrowserFile file, CancellationToken token)
        {
            return await UploadFile(file, "ItemGroup", token);
        }

        public async Task<Response<Guid?>> UploadItemCategories(IBrowserFile file, CancellationToken token)
        {
            return await UploadFile(file, "ItemCategory", token);
        }

        public async Task<Response<Guid?>> UploadItemUnits(IBrowserFile file, CancellationToken token)
        {
            return await UploadFile(file, "Item", token);
        }

        public async Task<Response<Guid?>> UploadItems(IBrowserFile file, CancellationToken token)
        {
            return await UploadFile(file, "Item", token);
        }
    }
}
