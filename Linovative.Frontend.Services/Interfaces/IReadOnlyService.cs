using Linovative.Frontend.Services.Models;
using LinoVative.Shared.Dto.Commons;

namespace Linovative.Frontend.Services.Interfaces
{
    public interface IReadOnlyService<T>
    {
        Task<Response<List<T>>> Get(CancellationToken token);
        Task<Response<T>> Get(Guid id, CancellationToken token, string? odataOption = default);
        Task<Response<List<T>>> Get(ODataFilter oDataFilter, CancellationToken token);
        Task<Response<List<T>>> Get(List<FilterCondition> filterConditions, ODataFilter oDataFilter, CancellationToken token);
        Task<Response<List<T>>> Get(ODataFilter oDataFilter, object filterObject, CancellationToken token);
        Task<Response<List<T>>> GetAll(ODataFilter oDataFilter, CancellationToken token);
        Task<Response<List<T>>> GetAll(List<FilterCondition> filterConditions, ODataFilter oDataFilter, CancellationToken token);
        Task<Response<List<T>>> GetAll(CancellationToken token);
    }
}
