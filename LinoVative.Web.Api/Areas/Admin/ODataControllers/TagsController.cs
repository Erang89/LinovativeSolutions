using LinoVative.Service.Backend.CrudServices.Items.Tags;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.ItemDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Net;

namespace LinoVative.Web.Api.Areas.Admin.ODataControllers
{

    public class TagsController : PrivateODataBaseController
    {
        private readonly ILogger _logger;
        public TagsController(ILogger<TagsController> log) => _logger = log;



        [EnableQuery]
        [ProducesResponseType(typeof(APIListResponse<TagDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] GetAllTagIQueryableCommand c, CancellationToken token)
        {
            try
            {
                IQueryable<TagDto> result = await _mediator.Send(c, token);
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
