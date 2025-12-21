using Linovative.Frontend.Services.Extensions;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto;
using LinoVative.Shared.Dto.Commons;
using LinoVative.Shared.Dto.MasterData.Shifts;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Linovative.Frontend.Services.FrontendServices.BaseServices
{
    public abstract class CrudServiceAbstract<T> : ReadOnlyServiceAbstract<T>, ICrudInterfaces
    {
        protected CrudServiceAbstract(IHttpClientFactory httpFactory, ILogger logger, string uriPrefix) : base(httpFactory, logger, uriPrefix)
        {

        }


        public virtual async Task<Response<T>> GetForUpdateByID<T>(Guid id, CancellationToken token) where T : class
        {
            try
            {
                var url = $"{_uriPrefix}/{id}";
                var httpResponse = await _httpClient.GetAsync(url, token);
                var response = await httpResponse.ToAppResponse<T>(token);
                if (response) return response!;

                return Response<T>.Failed(response.Title, response.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<T>.Failed(Messages.GeneralErrorMessage);
            }
        }

        // Create 
        public virtual async Task<Response> Create(object obj, CancellationToken token)
        {
            try
            {
                var url = $"{_uriPrefix}/{CREATE}";

                var response = await _httpClient.PostAsJsonAsync(url, obj, token);
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

                var response = await _httpClient.PutAsJsonAsync(url, obj, token);
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
                var response = await _httpClient.PostAsJsonAsync(url, obj, token);
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
                var response = await _httpClient.PostAsJsonAsync(url, obj, token);
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
