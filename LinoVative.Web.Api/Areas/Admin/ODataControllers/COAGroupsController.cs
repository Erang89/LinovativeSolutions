using LinoVative.Service.Backend.CrudServices.COAGroups;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.MasterData.Accountings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Net;

namespace LinoVative.Web.Api.Areas.Admin.ODataControllers
{

    public class COAGroupsController : PrivateODataBaseController
    {
        private readonly ILogger _logger;
        public COAGroupsController(ILogger<COAGroupsController> log) => _logger = log;



        [EnableQuery]
        [ProducesResponseType(typeof(APIListResponse<COAGroupDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] GetAllCOAGroupIQueryableCommand c, CancellationToken token)
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
