using LinoVative.Service.Backend.CrudServices.Items.ItemGroups;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.ItemDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Net;

namespace LinoVative.Web.Api.Areas.Admin.ODataControllers
{

    public class ItemGroupsController : MainApiODataBaseController
    {
        private readonly ILogger _logger;
        public ItemGroupsController(ILogger<ItemGroupsController> log) => _logger = log;



        [EnableQuery]
        [ProducesResponseType(typeof(APIListResponse<ItemGroupDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] GetAllItemGroupIQueryableCommand c, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(c, token);
                return Ok(result);
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
