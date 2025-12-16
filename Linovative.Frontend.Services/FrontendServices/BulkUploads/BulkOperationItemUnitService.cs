using Linovative.Frontend.Services.Extensions;
using Linovative.Frontend.Services.FrontendServices.BaseServices;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using Linovative.Shared.Interface.Enums;
using LinoVative.Shared.Dto.BulkUploads;
using LinoVative.Shared.Dto.Commons;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace Linovative.Frontend.Services.BulkUploads
{
    public interface IBulkOperationItemUnitService : IReadOnlyService<BulkUploadItemUnitDto>, ICrudInterfaces
    {
        public Task<Response> Remove(CrudOperations operation, CancellationToken token);
        public Task DownloadUpdateTemplate(List<FilterCondition> filter);
        public Task DownloadDeleteTemplate(List<FilterCondition> filter);
        public Task<Response> Save(CrudOperations operation, IDictionary<string, string> fieldMapping, CancellationToken token);

    }

    public class BulkOperationItemUnitService : CrudServiceAbstract<BulkUploadItemUnitDto>, IBulkOperationItemUnitService
    {

        private readonly IJSRuntime _js;

        public BulkOperationItemUnitService(IJSRuntime js, IHttpClientFactory httpFactory, ILogger<BulkOperationItemUnitService> logger) : base(httpFactory, logger, "BulkOperationItemUnits")
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

        public Task<Response> Remove(CrudOperations operation, CancellationToken token) => Delete(operation.ToString(), token);

        public async Task DownloadUpdateTemplate(List<FilterCondition> filter)
        {
            try
            {
                var url = $"{_uriPrefix}/Download/updatetemplate";
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
                var finalFileName = $"UnitsForUpdate_{timestamp}.xlsx";

                var base64 = Convert.ToBase64String(data);
                await _js.InvokeVoidAsync("saveAsFile", finalFileName, base64);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file");
            }
        }


        public async Task DownloadDeleteTemplate(List<FilterCondition> filter)
        {
            try
            {
                var url = $"{_uriPrefix}/Download/deletetemplate";
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
                var finalFileName = $"UnitsForDelete_{timestamp}.xlsx";

                var base64 = Convert.ToBase64String(data);
                await _js.InvokeVoidAsync("saveAsFile", finalFileName, base64);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file");
            }
        }

        public async Task<Response> Save(CrudOperations operation, IDictionary<string, string> fieldMapping, CancellationToken token)
        {
            try
            {
                var httpResponse = await _httpClient.PostAsJsonAsync($"{_uriPrefix}/save/{operation}", fieldMapping, token);
                var response = await httpResponse.ToAppBoolResponse(token);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.Failed(Messages.GeneralErrorMessage);
            }
        }
    }
}
