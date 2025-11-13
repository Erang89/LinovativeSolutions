using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.Extensions.Localization;

namespace LinoVative.Web.Api.Areas.Admin.Controllers.Resources
{
    public class ResourcesController : PrivateAPIBaseController
    {
        private readonly IStringLocalizer _loc;
        public ResourcesController(IMediator mediator, ILogger<ResourcesController> logger, IStringLocalizer loc) : base(mediator, logger)
        {
            _loc = loc;
        }

        [Route("Excel/{fileName}")]
        [HttpGet]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadExcelFile([FromRoute]string fileName, CancellationToken token)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "ExcelFiles", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                var result = Result.Failed(string.Format(DISPLAY_ERROR_MESSAGE, "Download file"));
                return StatusCode((int)result.Status, result);
            }


            try
            {
                var bytes = await System.IO.File.ReadAllBytesAsync(filePath, token);
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(bytes, contentType, fileName.Replace(".xlsx", $"_{DateTime.Now.ToString("yyyyMMddhhmmss")}.xlsx"));
            }
            catch (Exception ex)
            {
                var routeName = ControllerContext.ActionDescriptor.DisplayName;
                _logger.LogError(ex, LOG_ERRROR_MESSAGE, routeName);
                var responseObject = Result.Failed(string.Format(DISPLAY_ERROR_MESSAGE, routeName));
                responseObject.SetTraceId(HttpContext.TraceIdentifier);
                return StatusCode((int)HttpStatusCode.InternalServerError, responseObject)!;
            }
        }
    }
}
