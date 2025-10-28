using Linovative.Frontend.Services.Extensions;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.Commons;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Linovative.Frontend.Services.FrontendServices
{
    internal abstract class CrudServiceAbstract<T>
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        const string CREATE = "create";
        const string UPDATE = "update";
        const string DELETE = "delete";

        private readonly string _uriPrefix;

        protected CrudServiceAbstract(
            IHttpClientFactory httpFactory,
            ILogger logger,
            string uriPrefix
            )
        {
            _logger = logger;
            _httpClient = httpFactory.CreateClient(EndpointNames.API);
            _uriPrefix = uriPrefix;
        }


        // Create 
        protected async Task<Response<Guid>> Create(object obj, CancellationToken token)
        {
            try
            {
                var url = $"{_uriPrefix}/{CREATE}";

                var response = await _httpClient.PostAsJsonAsync(url, obj, token);
                return await response.ToAppResponse<Guid>(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<Guid>.Failed(Messages.GeneralErrorMessage);
            }
        }

        //Update
        protected async Task<Response<Guid>> Update(object obj, CancellationToken token)
        {
            try
            {
                var url = $"{_uriPrefix}/{UPDATE}";

                var response = await _httpClient.PutAsJsonAsync(url, obj, token);
                return await response.ToAppResponse<Guid>(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<Guid>.Failed(Messages.GeneralErrorMessage);
            }
        }


        //Delete
        protected async Task<Response<Guid>> Delete(Guid id, CancellationToken token)
        {
            try
            {
                var url = $"{_uriPrefix}/{DELETE}";
                var obj = new BulkDeleteDto() { Ids = new() { id } };
                var response = await _httpClient.PostAsJsonAsync(url, obj, token);
                return await response.ToAppResponse<Guid>(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<Guid>.Failed(Messages.GeneralErrorMessage);
            }
        }


        //Delete List
        protected async Task<Response<bool>> Delete(object obj, CancellationToken token)
        {
            try
            {
                var url = $"{_uriPrefix}/{DELETE}";
                var response = await _httpClient.PostAsJsonAsync(url, obj, token);
                return await response.ToAppResponse<bool>(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<bool>.Failed(Messages.GeneralErrorMessage);
            }
        }


        protected async Task<Response<List<T>>> Get(CancellationToken token)
        {
            try
            {
                var url = $"oData/{_uriPrefix}?$count=true";
                var response = await _httpClient.GetAsync(url, token);
                return await response.ToAppResponse<List<T>>(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<List<T>>.Failed(Messages.GeneralErrorMessage);
            }
        }

        protected async Task<Response<T>> Get(Guid id, CancellationToken token, string? odataOption = default)
        {
            try
            {
                odataOption = odataOption is not null ? $"&{odataOption}" : null;
                var url = $"oData/{_uriPrefix}?$filter=id eq ({id}){odataOption}";
                var httpResponse = await _httpClient.GetAsync(url, token);
                var response = await httpResponse.ToAppResponse<List<T>>(token);
                if (response)
                    return Response<T>.Ok((response.Data ?? new()).FirstOrDefault());

                return Response<T>.Failed(response.Title, response.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<T>.Failed(Messages.GeneralErrorMessage);
            }
        }

        // ODataQuery
        protected async Task<Response<List<T>>> Get(ODataFilter oDataFilter, CancellationToken token)
        {
            try
            {
                var url = oDataFilter.GetODataUrl(_uriPrefix);
                var response = await _httpClient.GetAsync(url, token);
                return await response.ToAppResponse<List<T>>(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<List<T>>.Failed(Messages.GeneralErrorMessage);
            }
        }

        protected async Task<Response<List<T>>> Get(List<FilterCondition> filterConditions, ODataFilter oDataFilter, CancellationToken token)
        {
            try
            {
                var url = oDataFilter.GetODataUrl(_uriPrefix);
                var response = await _httpClient.PostAsJsonAsync(url, new { FilterConditions = filterConditions }, token);
                return await response.ToAppResponse<List<T>>(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<List<T>>.Failed(Messages.GeneralErrorMessage);
            }
        }


        // ODataQuery
        protected async Task<Response<List<T>>> Get(ODataFilter oDataFilter, object filterObject, CancellationToken token)
        {
            try
            {
                var url = oDataFilter.GetODataUrl(_uriPrefix);
                var response = await _httpClient.PostAsJsonAsync(url, filterObject, token);
                return await response.ToAppResponse<List<T>>(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<List<T>>.Failed(Messages.GeneralErrorMessage);
            }
        }


        // OData Query Get All
        protected virtual async Task<Response<List<T>>> GetAll(ODataFilter filter, CancellationToken token)
        {
            var result = await Get(filter, token);

            if (!result || result.Count <= filter.PageSize)
                return result;

            var data = result.Data!;

            int pageCount = result.Count / filter.PageSize;
            pageCount += result.Count % filter.PageSize > 0 ? 1 : 0;

            var tasks = new List<Task<Response<List<T>>>>();

            for (var page = 2; page <= pageCount; page++)
            {
                filter.PageNumber = page;
                tasks.Add(Get(filter, token));
            }

            var results = await Task.WhenAll(tasks);

            foreach (var taskResult in results)
            {
                if (taskResult && taskResult.Data != null)
                {
                    data.AddRange(taskResult.Data);
                }
            }

            return Response<List<T>>.Ok(data);
        }

        protected virtual async Task<Response<List<T>>> GetAll(List<FilterCondition> filterConditions, ODataFilter filter, CancellationToken token)
        {
            var result = await Get(filterConditions, filter, token);

            if (!result || result.Count <= filter.PageSize)
                return result;

            var data = result.Data!;

            int pageCount = result.Count / filter.PageSize;
            pageCount += result.Count % filter.PageSize > 0 ? 1 : 0;

            var tasks = new List<Task<Response<List<T>>>>();

            for (var page = 2; page <= pageCount; page++)
            {
                filter.PageNumber = page;
                tasks.Add(Get(filterConditions, filter, token));
            }

            var results = await Task.WhenAll(tasks);

            foreach (var taskResult in results)
            {
                if (taskResult && taskResult.Data != null)
                {
                    data.AddRange(taskResult.Data);
                }
            }

            return Response<List<T>>.Ok(data);
        }


        protected virtual Task<Response<List<T>>> GetAll(CancellationToken token)
        {
            return GetAll(new ODataFilter() { PageNumber = 1, PageSize = 100 }, token);
        }


    }

    internal class ReadOnlyService<T> : CrudServiceAbstract<T>, IReadOnlyService<T>
    {
        public ReadOnlyService(
           IHttpClientFactory httpFactory,
           ILogger logger,
           string uriPrefix) : base(httpFactory, logger, uriPrefix)
        {

        }

        public new Task<Response<List<T>>> Get(ODataFilter oDataFilter, CancellationToken token) => base.Get(oDataFilter, token);
        public new Task<Response<List<T>>> Get(List<FilterCondition> filterConditions, ODataFilter oDataFilter, CancellationToken token) => base.Get(filterConditions, oDataFilter, token);
        public new Task<Response<List<T>>> Get(ODataFilter oDataFilter, object filterObject, CancellationToken token) => base.Get(oDataFilter, filterObject, token);
        public new Task<Response<List<T>>> Get(CancellationToken token) => base.Get(token);
        public new Task<Response<T>> Get(Guid id, CancellationToken token, string? odataOption = default) => base.Get(id, token, odataOption);
        public new Task<Response<List<T>>> GetAll(ODataFilter oDataFilter, CancellationToken token) => base.GetAll(oDataFilter, token);
        public new Task<Response<List<T>>> GetAll(List<FilterCondition> filterConditions, ODataFilter oDataFilter, CancellationToken token) => base.GetAll(filterConditions, oDataFilter, token);
        public new Task<Response<List<T>>> GetAll(CancellationToken token) => base.GetAll(token);
    }

    internal class CrudServiceBase<T> : CrudServiceAbstract<T>, ICrudInterfaces<T>
    {
        public CrudServiceBase(
            IHttpClientFactory httpFactory,
            ILogger logger,
            string uriPrefix) : base(httpFactory, logger, uriPrefix)
        {

        }


        public new Task<Response<Guid>> Create(object obj, CancellationToken token) => base.Create(obj, token);
        public new Task<Response<Guid>> Update(object obj, CancellationToken token) => base.Update(obj, token);
        public new Task<Response<Guid>> Delete(Guid id, CancellationToken token) => base.Delete(id, token);
        public Task<Response<bool>> Delete(List<Guid> ids, CancellationToken token) => base.Delete(new BulkDeleteDto() { Ids = ids }, token);
        public new Task<Response<List<T>>> Get(ODataFilter oDataFilter, CancellationToken token) => base.Get(oDataFilter, token);
        public new Task<Response<List<T>>> Get(ODataFilter oDataFilter, object filterObject, CancellationToken token) => base.Get(oDataFilter, filterObject, token);
        public new Task<Response<List<T>>> Get(CancellationToken token) => base.Get(token);
        public new Task<Response<T>> Get(Guid id, CancellationToken token, string? odataOption = default) => base.Get(id, token, odataOption);
        public new Task<Response<List<T>>> Get(List<FilterCondition> filterConditions, ODataFilter oDataFilter, CancellationToken token) => base.Get(filterConditions, oDataFilter, token);
        public new Task<Response<List<T>>> GetAll(ODataFilter oDataFilter, CancellationToken token) => base.GetAll(oDataFilter, token);
        public new Task<Response<List<T>>> GetAll(List<FilterCondition> filterConditions, ODataFilter oDataFilter, CancellationToken token) => base.GetAll(filterConditions, oDataFilter, token);
        public new Task<Response<List<T>>> GetAll(CancellationToken token) => base.GetAll(token);
    }

}
