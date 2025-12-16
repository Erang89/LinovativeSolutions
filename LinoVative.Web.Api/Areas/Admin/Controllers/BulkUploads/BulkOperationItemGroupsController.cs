using Linovative.Shared.Interface.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Delete;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.DeleteTemplateWithData;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.UpdateTemplateWithData;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Enums;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.SaveOperations;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperations.Download.ErrorRecords;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LinoVative.Web.Api.Areas.Admin.Controllers.BulkUploads
{
    public class BulkOperationItemGroupsController : PrivateAPIBaseController
    {
        public BulkOperationItemGroupsController(IMediator mediator, ILogger<BulkOperationItemGroupsController> logger) : base(mediator, logger)
        {

        }

        [Route("Upload")]
        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Upload(BulkUploadItemGroupCommand c, CancellationToken token)
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
        public async Task<IActionResult> Download([FromBody]DownloadItemGroupForUpdateCommand c, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(c, token);
                var ms = (MemoryStream)result.Data!;

                if(!result)
                    return StatusCode((int)result.Status, result);

                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ItemGroups.xlsx");
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
        public async Task<IActionResult> DownloadDeleteTemplate([FromBody] DownloadItemGroupForDeleteCommand c, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(c, token);
                var ms = (MemoryStream)result.Data!;

                if (!result)
                    return StatusCode((int)result.Status, result);

                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ItemGroups.xlsx");
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
        public async Task<IActionResult> Remove([FromRoute]CrudOperations? type, CancellationToken token)
        {
            try
            {
                var removeBulkUploadType = type switch
                {
                    CrudOperations.Create => BulkOperationTypes.GroupCreate,
                    CrudOperations.Update => BulkOperationTypes.GroupUpdate,
                    CrudOperations.Delete => BulkOperationTypes.GroupDelete,
                    CrudOperations.Mapping => BulkOperationTypes.GroupMapping,
                    _ => BulkOperationTypes.GroupMapping,
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


        [Route("Save/{type}")]
        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Save([FromBody] Dictionary<string, string> fieldMapping, [FromRoute] CrudOperations? type, CancellationToken token)
        {
            try
            {
                var bulkOperationType = type switch
                {
                    CrudOperations.Create => BulkOperationTypes.GroupCreate,
                    CrudOperations.Update => BulkOperationTypes.GroupUpdate,
                    CrudOperations.Delete => BulkOperationTypes.GroupDelete,
                    _ => throw new NotImplementedException(),
                };
                var c = new BulkOperationProcessCommand() { Type = bulkOperationType, FieldMapping = fieldMapping };
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


        [Route("Error/Download/{type}")]
        [HttpGet]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ErrorDownload([FromRoute] CrudOperations? type, CancellationToken token)
        {
            try
            {
                var bulkOperationType = type switch
                {
                    CrudOperations.Create => BulkOperationTypes.GroupCreate,
                    CrudOperations.Update => BulkOperationTypes.GroupUpdate,
                    CrudOperations.Delete => BulkOperationTypes.GroupDelete,
                    _ => throw new NotImplementedException(),
                };
                var c = new DownloadErrorRecordCommand() { Type = bulkOperationType };
                var result = await _mediator.Send(c, token);


                if (!result)
                    return StatusCode((int)result.Status, result);

                var ms = (MemoryStream)result.Data!;
                return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ErrorGroups_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");

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
