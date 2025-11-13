using Linovative.Frontend.Services.Models;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Linovative.Frontend.Services.FrontendServices.Resources
{
    public interface IResourceDownload
    {
        public Task DownloadExcelFile(string fileName);
    }

    public class ResourceDownloadService : IResourceDownload
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly string _uriPrefix = "Resources";
        private readonly IJSRuntime _js;

        public ResourceDownloadService(IHttpClientFactory httpFactory,
            ILogger<ResourceDownloadService> logger, IJSRuntime js)
        {
            _logger = logger;
            _httpClient = httpFactory.CreateClient(EndpointNames.PrivateApi);
            _js = js;
        }
        public async Task DownloadExcelFile(string fileName)
        {
            try
            {
                var url = $"{_uriPrefix}/Excel/{fileName}";

                var stream = await _httpClient.GetStreamAsync(url);
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                var byteArray = ms.ToArray();

                await _js.InvokeVoidAsync("saveAsFile", fileName.Replace(".xlsx", $"_{DateTime.Now.ToString("yyyyMMddhhmmss")}.xlsx"), Convert.ToBase64String(byteArray));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
