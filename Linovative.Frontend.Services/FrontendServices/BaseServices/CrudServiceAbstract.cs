using Linovative.Frontend.Services.Extensions;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.Commons;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Linovative.Frontend.Services.FrontendServices.BaseServices
{

    internal abstract class CrudServiceAbstract<T> : ReadOnlyServiceAbstract<T>, ICrudInterfaces
    {
        protected CrudServiceAbstract(IHttpClientFactory httpFactory, ILogger logger, string uriPrefix) : base(httpFactory, logger, uriPrefix)
        {

        }


        // Create 
        public virtual async Task<Response> Create(object obj, CancellationToken token)
        {
            try
            {
                var url = $"{_uriPrefix}/{CREATE}";

                var response = await _privateHttpClient.PostAsJsonAsync(url, obj, token);
                return await response.ToAppBoolResponse(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.Failed(Messages.GeneralErrorMessage);
            }
        }

        //Update
        public virtual async Task<Response> Update(object obj, CancellationToken token)
        {
            try
            {
                var url = $"{_uriPrefix}/{UPDATE}";

                var response = await _privateHttpClient.PutAsJsonAsync(url, obj, token);
                return await response.ToAppBoolResponse(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.Failed(Messages.GeneralErrorMessage);
            }
        }


        //Delete
        public virtual async Task<Response> Delete(Guid id, CancellationToken token)
        {
            try
            {
                var url = $"{_uriPrefix}/{DELETE}";
                var obj = new BulkDeleteDto() { Ids = new() { id } };
                var response = await _privateHttpClient.PostAsJsonAsync(url, obj, token);
                return await response.ToAppBoolResponse(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.Failed(Messages.GeneralErrorMessage);
            }
        }


        //Delete List
        public virtual async Task<Response> Delete(object obj, CancellationToken token)
        {
            try
            {
                var url = $"{_uriPrefix}/{DELETE}";
                var response = await _privateHttpClient.PostAsJsonAsync(url, obj, token);
                return await response.ToAppBoolResponse(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.Failed(Messages.GeneralErrorMessage);
            }
        }

    }
}
