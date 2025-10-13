using LinoVative.Service.Backend.CrudServices.Companies;
using LinoVative.Service.Core.Commons;
using LinoVative.Service.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LinoVative.Web.Api.Areas.Admin.Controllers.Companies
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public CompaniesController(IMediator mediator, ILogger<CompaniesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterNew(RegisterNewCompanyServiceCommand c, CancellationToken token)
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
