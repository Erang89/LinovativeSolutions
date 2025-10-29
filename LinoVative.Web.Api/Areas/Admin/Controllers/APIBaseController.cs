using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using LinoVative.Web.Api.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinoVative.Web.Api.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(APIInputErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(APIInternalErrorResponse), StatusCodes.Status500InternalServerError)]
    [Authorize(AuthenticationSchemes = AppSchemeNames.MainAPIScheme)]
    public abstract class APIBaseController : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly ILogger _logger;

        public APIBaseController(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        protected const string LOG_ERRROR_MESSAGE = "An error occurred in {Route}";
        protected const string DISPLAY_ERROR_MESSAGE = "An error occurred while handling {routeName}. Please contact your administrator";
        protected const string CREATE = "Create";
        protected const string UPDATE = "Update";
        protected const string DELETE = "Delete/{id}";
        protected const string GETALL = "Getall";

    }
}
