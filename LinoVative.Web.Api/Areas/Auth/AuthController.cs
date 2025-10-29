using LinoVative.Service.Backend.AuthServices;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LinoVative.Web.Api.Areas.Auth
{
    [Route("public/api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        protected const string LOG_ERRROR_MESSAGE = "An error occurred in {Route}";
        protected const string DISPLAY_ERROR_MESSAGE = "An error occurred while handling {routeName}. Please contact your administrator";

        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;

        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand c, CancellationToken token)
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
    }
}
