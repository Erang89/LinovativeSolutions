using LinoVative.Service.Backend.AuthServices;
using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Web.Api.Areas.Admin.Controllers;
using LinoVative.Web.Api.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LinoVative.Web.Api.Areas.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : MediatorControllerBase
    {
        public AuthController(IMediator mediator, ILogger<AuthController> logger) : base(mediator, logger)
        {
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand c, CancellationToken token)
        {
            try
            {
                c.SetRequestIP(GetClientIp());
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


        [HttpPost]
        [Route("SelectCompany")]
        [Authorize(AuthenticationSchemes = AppSchemeNames.CommonApiScheme)]
        public async Task<IActionResult> ChangeDefaultCompany([FromBody] ChangeDefaultCompanyCommand c, CancellationToken token)
        {
            try
            {
                c.SetRequestIP(GetClientIp());
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


        [HttpPost]
        [Route("Jwt/Refresh")]
        [Authorize(AuthenticationSchemes = AppSchemeNames.CommonApiScheme)]
        public async Task<IActionResult> JwtRefresh([FromBody] RefreshTokenCommand c, CancellationToken token)
        {
            try
            {
                c.SetRequestIP(GetClientIp());
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

        string GetClientIp()
        {
            var remoteIp = HttpContext.Connection.RemoteIpAddress;
            return remoteIp?.MapToIPv4().ToString() ?? "0.0.0.0";
        }
    }
}
