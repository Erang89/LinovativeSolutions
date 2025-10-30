﻿using LinoVative.Service.Core.Interfaces;
using LinoVative.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using LinoVative.Service.Backend.CrudServices.Accounts;

namespace LinoVative.Web.Api.Areas.Admin.Controllers.Accountings
{
    public class AccountsController : APIBaseController
    {
        public AccountsController(IMediator mediator, ILogger<AccountsController> logger) : base(mediator, logger)
        {
        }

        [Route(CREATE)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateAccountCommand c, CancellationToken token)
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


        [Route(UPDATE)]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UpdateAccountCommand c, CancellationToken token)
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


        [Route(DELETE)]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute]Guid id, CancellationToken token)
        {
            try
            {
                var result = await _mediator.Send(new DeleteAccountCommand() { Id = id}, token);
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
