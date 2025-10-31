using Linovative.Frontend.Services.Extensions;
using Linovative.Frontend.Services.Interfaces;
using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.Commons;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Linovative.Frontend.Services.FrontendServices.BaseServices
{
    public abstract class ReadOnlyServiceAbstract<T> : IReadOnlyService<T>
    {
        protected HttpClient _httpClient => IsPublicEndpoint? _publicHttpClient : _privateHttpClient ?? new HttpClient();
        protected readonly HttpClient _privateHttpClient;
        protected readonly HttpClient _publicHttpClient;
        protected readonly ILogger _logger;

        protected const string CREATE = "create";
        protected const string UPDATE = "update";
        protected const string DELETE = "delete";

        protected readonly string _uriPrefix;

        protected abstract bool IsPublicEndpoint { get; }

        protected ReadOnlyServiceAbstract(
            IHttpClientFactory httpFactory,
            ILogger logger,
            string uriPrefix
            )
        {
            _logger = logger;
            _privateHttpClient = httpFactory.CreateClient(EndpointNames.PrivateApi);
            _publicHttpClient = httpFactory.CreateClient(EndpointNames.PublicApi);
            _uriPrefix = uriPrefix;
        }



        public virtual async Task<Response<List<T>>> Get(CancellationToken token)
        {
            try
            {
                var url = $"oData/{_uriPrefix}?$count=true";
                var response = await _httpClient.PostAsJsonAsync(url, token);
                return await response.ToAppResponse<List<T>>(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<List<T>>.Failed(Messages.GeneralErrorMessage);
            }
        }

        public virtual async Task<Response<T>> Get(Guid id, CancellationToken token, string? odataOption = default)
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
        public virtual async Task<Response<List<T>>> Get(ODataFilter oDataFilter, CancellationToken token)
        {
            try
            {
                var url = oDataFilter.GetODataUrl(_uriPrefix);
                var response = await _httpClient.PostAsJsonAsync(url, oDataFilter.FilterPayload, token);
                return await response.ToAppResponse<List<T>>(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<List<T>>.Failed(Messages.GeneralErrorMessage);
            }
        }

        public virtual async Task<Response<List<T>>> Get(List<FilterCondition> filterConditions, ODataFilter oDataFilter, CancellationToken token)
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
        public virtual async Task<Response<List<T>>> Get(ODataFilter oDataFilter, object filterObject, CancellationToken token)
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
        public virtual async Task<Response<List<T>>> GetAll(ODataFilter filter, CancellationToken token)
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

        public virtual async Task<Response<List<T>>> GetAll(List<FilterCondition> filterConditions, ODataFilter filter, CancellationToken token)
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


        public virtual Task<Response<List<T>>> GetAll(CancellationToken token)
        {
            return GetAll(new ODataFilter() { PageNumber = 1, PageSize = 100 }, token);
        }


    }


}
