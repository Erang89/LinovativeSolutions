using LinoVative.Service.Backend.CrudServices.Sources;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Commons;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LinoVative.Web.Api.Areas.Admin.Controllers.Sources
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(APIInputErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(APIInternalErrorResponse), StatusCodes.Status500InternalServerError)]
    public class SourcesController : APIBaseController
    {
        public SourcesController(IMediator mediator, ILogger<SourcesController> logger)
            : base(mediator, logger)
        { }



        [Route("Currencies")]
        [HttpPost]
        [ProducesResponseType(typeof(APIListResponse<IdWithCodeDto>), StatusCodes.Status200OK)]
        
        public async Task<IActionResult> Currencies(GetAllCurrencyCommand c, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(c, token);
                return StatusCode((int)result.Status, result);
            }
            catch (Exception ex)
            {
                var routeName = ControllerContext.ActionDescriptor.DisplayName;
                _logger.LogError(ex, "An error occurred in {Route}", routeName);
                var responseObject = Result.Failed($"An error occurred while handling {routeName}. Please contact your administrator");
                responseObject.SetTraceId(HttpContext.TraceIdentifier);
                return StatusCode((int)HttpStatusCode.InternalServerError, responseObject)!;
            }
        }


        [Route("Countries")]
        [HttpPost]
        [ProducesResponseType(typeof(APIListResponse<IdWithCodeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Countries(GetAllCountryCommand c, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(c, token);
                return StatusCode((int)result.Status, result);
            }
            catch (Exception ex)
            {
                var routeName = ControllerContext.ActionDescriptor.DisplayName;
                _logger.LogError(ex, "An error occurred in {Route}", routeName);
                var responseObject = Result.Failed($"An error occurred while handling {routeName}. Please contact your administrator");
                responseObject.SetTraceId(HttpContext.TraceIdentifier);
                return StatusCode((int)HttpStatusCode.InternalServerError, responseObject)!;
            }
        }


        [Route("Timezones")]
        [HttpPost]
        [ProducesResponseType(typeof(APIListResponse<IdWithCodeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Timezones(GetAllTimezoneCommand c, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(c, token);
                return StatusCode((int)result.Status, result);
            }
            catch (Exception ex)
            {
                var routeName = ControllerContext.ActionDescriptor.DisplayName;
                _logger.LogError(ex, "An error occurred in {Route}", routeName);
                var responseObject = Result.Failed($"An error occurred while handling {routeName}. Please contact your administrator");
                responseObject.SetTraceId(HttpContext.TraceIdentifier);
                return StatusCode((int)HttpStatusCode.InternalServerError, responseObject)!;
            }
        }
    }
}
