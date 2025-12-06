using Linovative.Frontend.Services.Extensions;
using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Models;
using Linovative.Shared.Interface.Enums;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace Linovative.Frontend.Services.BulkUploads
{
    public interface IBulkUploadService 
    {
        public Task<Response<Guid?>> UploadItemGroups(IBrowserFile file, CrudOperations operation, CancellationToken token);
        public Task<Response<Guid?>> UploadItemCategories(IBrowserFile file, CrudOperations operation, CancellationToken token);
        public Task<Response<Guid?>> UploadItemUnits(IBrowserFile file, CrudOperations operation, CancellationToken token);
        public Task<Response<Guid?>> UploadItems(IBrowserFile file, CrudOperations operation, CancellationToken token);
    }

    public class BulkUploadService : RequeserServiceBase, IBulkUploadService
    {

        private readonly long maxFileSize = 20 * 1024 * 1024; // 20 MB

        public BulkUploadService(IHttpClientFactory httpFactory, ILogger<BulkUploadService> logger) : base(httpFactory, logger, "/")
        {
        }

        private async Task<Response<Guid?>> UploadFile(IBrowserFile file, CrudOperations operation, string endpoint, CancellationToken token)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                using var stream = file!.OpenReadStream(maxFileSize);
                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType ?? "application/octet-stream");
                content.Add(streamContent, "file", file.Name);
                content.Add(new StringContent(operation.ToString()), "operation");
                var httpResponse = await _httpClient.PostAsync($"{endpoint}/upload", content, token);
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

        public async Task<Response<Guid?>> UploadItemGroups(IBrowserFile file, CrudOperations operation, CancellationToken token)
        {
            return await UploadFile(file, operation, "BulkUploadItemGroups",  token);
        }

        public async Task<Response<Guid?>> UploadItemCategories(IBrowserFile file, CrudOperations operation, CancellationToken token)
        {
            return await UploadFile(file, operation, "BulkUploadItemCategories", token);
        }

        public async Task<Response<Guid?>> UploadItemUnits(IBrowserFile file, CrudOperations operation, CancellationToken token)
        {
            return await UploadFile(file, operation, "BulkUploadItemUnits", token);
        }

        public async Task<Response<Guid?>> UploadItems(IBrowserFile file, CrudOperations operation, CancellationToken token)
        {
            return await UploadFile(file, operation, "BulkUploadItems", token);
        }
    }
}
