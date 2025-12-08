using Linovative.Frontend.Services.Extensions;
using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.BulkUploads;
using LinoVative.Shared.Dto.Commons;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace Linovative.Frontend.Services.BulkUploads
{
    public interface IBulkOperationItemGroupService : IReadOnlyService<BulkUploadItemGroupDto>, ICrudInterfaces
    {
        public Task<Response> RemoveBulkCreate(CancellationToken token);
        public Task<Response> RemoveBulkUpdate(CancellationToken token);
        public Task<Response> RemoveBulkDelete(CancellationToken token);
        public Task Download(List<FilterCondition> filter);
    }

    public class BulkOperationItemGroupService : CrudServiceAbstract<BulkUploadItemGroupDto>, IBulkOperationItemGroupService
    {

        private readonly IJSRuntime _js;

        public BulkOperationItemGroupService(IJSRuntime js, IHttpClientFactory httpFactory, ILogger<BulkOperationItemGroupService> logger) : base(httpFactory, logger, "BulkOperationItemGroups")
        { 
            _js = js;
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

        public async Task Download(List<FilterCondition> filter)
        {
            try
            {
                var url = $"{_uriPrefix}/Download";
                var filterObject = new { filter };

                using var response = await _httpClient.PostAsJsonAsync(url, filterObject);

                if (!response.IsSuccessStatusCode)
                {
                    var respText = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Download failed. Status: {StatusCode}. Response: {Response}", response.StatusCode, respText);
                    return;
                }

                var data = await response.Content.ReadAsByteArrayAsync();
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var finalFileName = $"Items_{timestamp}.xlsx";

                var base64 = Convert.ToBase64String(data);
                await _js.InvokeVoidAsync("saveAsFile", finalFileName, base64);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file");
            }
        }

    }
}
