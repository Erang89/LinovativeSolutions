using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Delete;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Download.DeleteTemplateWithData;
using LinoVative.Service.Backend.CrudServices.Items.BulkUploads.Download.UpdateTemplateWithData;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LinoVative.Web.Api.Areas.Admin.Controllers.BulkUploads
{
    public class BulkOperationItemsController : PrivateAPIBaseController
    {
        public BulkOperationItemsController(IMediator mediator, ILogger<BulkOperationItemsController> logger) : base(mediator, logger)
        {

        }

        [Route("Upload")]
        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Upload(BulkUploadItemCommand c, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(c, token);
                return StatusCode((int)result.Status, result);
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


        [Route("Download/UpdateTemplate")]
        [HttpPost]
        [ProducesResponseType(typeof(File), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Download([FromBody] DownloadItemForUpdateCommand c, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(c, token);
                var ms = (MemoryStream)result.Data!;

                if (!result)
                    return StatusCode((int)result.Status, result);

                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Items.xlsx");
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


        [Route("Download/DeleteTemplate")]
        [HttpPost]
        [ProducesResponseType(typeof(File), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadDeleteTemplate([FromBody] DownloadItemForDeleteCommand c, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(c, token);
                var ms = (MemoryStream)result.Data!;

                if (!result)
                    return StatusCode((int)result.Status, result);

                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Items.xlsx");
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


        [Route("Remove/{type}")]
        [HttpDelete]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remove([FromRoute] CrudOperations? type, CancellationToken token)
        {
            try
            {
                var removeBulkUploadType = type switch
                {
                    CrudOperations.Create => RemoveBulkUploadItemType.ItemCreate,
                    CrudOperations.Update => RemoveBulkUploadItemType.ItemUpdate,
                    CrudOperations.Delete => RemoveBulkUploadItemType.ItemDelete,
                    CrudOperations.Mapping => RemoveBulkUploadItemType.ItemMapping,
                    _ => RemoveBulkUploadItemType.ItemMapping,
                };
                var c = new RemoveBulkUploadItemCommand() { UploadType = removeBulkUploadType };
                var result = await _mediator.Send(c, token);
                return StatusCode((int)result.Status, result);
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
