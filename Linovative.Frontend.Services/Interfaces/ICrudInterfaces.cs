using Linovative.Frontend.Services.Models;

namespace Linovative.Frontend.Services.Interfaces
{
    public interface ICrudInterfaces<T> : IReadOnlyService<T>
    {
        Task<Response> Create(object obj, CancellationToken token);
        Task<Response> Update(object obj, CancellationToken token);
        Task<Response> Delete(Guid id, CancellationToken token);
        Task<Response> Delete(List<Guid> ids, CancellationToken token);
    }
}
