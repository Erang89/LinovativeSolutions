using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates;
using LinoVative.Service.Backend.CrudServices.Items.BulkOperation.Download.BlankTemplates.Factory;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Net;

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
                var type = (new Dictionary<string, BulkOperationTemplateType>()
                {
                    {"CreateItemTemplate.xlsx", BulkOperationTemplateType.Item_Create },
                    {"UpdateItemTemplate.xlsx", BulkOperationTemplateType.Item_Update},
                    {"DeleteItemTemplate.xlsx", BulkOperationTemplateType.Item_Delete },

                    {"CreateItemGroupTemplate.xlsx", BulkOperationTemplateType.Group_Create },
                    {"UpdateGroupTemplate.xlsx", BulkOperationTemplateType.Group_Update},
                    {"DeleteGroupTemplate.xlsx", BulkOperationTemplateType.Group_Delete },

                    {"CreateItemCategoryTemplate.xlsx", BulkOperationTemplateType.Category_Create },
                    {"UpdateCategoryTemplate.xlsx", BulkOperationTemplateType.Category_Update},
                    {"DeleteCategoryTemplate.xlsx", BulkOperationTemplateType.Category_Delete },

                    {"CreateItemUnitTemplate.xlsx", BulkOperationTemplateType.Unit_Create },
                    {"UpdateUnitTemplate.xlsx", BulkOperationTemplateType.Unit_Update},
                    {"DeleteUnitTemplate.xlsx", BulkOperationTemplateType.Unit_Delete },
                })[fileName];

                var cmd = new BulkOperationExcelTemplateCommand() { Type = type };
                var result = await _mediator.Send(cmd, token);
                var ms = (MemoryStream)result.Data!;
                var bytes = ms.ToArray();
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
