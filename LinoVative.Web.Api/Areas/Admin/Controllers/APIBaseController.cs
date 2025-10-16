using LinoVative.Service.Core.Interfaces;
using LinoVative.Web.Api.Areas.Admin.Controllers.Companies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinoVative.Web.Api.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class APIBaseController : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly ILogger _logger;

        public APIBaseController(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

    }
}
