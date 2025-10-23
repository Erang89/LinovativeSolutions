using LinoVative.Service.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ODController = Microsoft.AspNetCore.OData.Routing.Controllers;


namespace LinoVative.Web.Api.Areas.Admin.ODataControllers
{
    [Controller]
    public class ODataControllerBaseController : ODController.ODataController
    {
        private IMediator? mediator;
        protected IMediator _mediator => mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;

        protected const string LOG_ERRROR_MESSAGE = "An error occurred in {Route}";
        protected const string DISPLAY_ERROR_MESSAGE = "An error occurred while handling {routeName}. Please contact your administrator";
    }
}
