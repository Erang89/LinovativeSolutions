using Linovative.Frontend.Services.Models;

namespace Linovative.Frontend.Services.Interfaces
{
    public interface ICrudInterfaces<T> : IReadOnlyService<T>
    {
        Task<Response<Guid>> Create(object obj, CancellationToken token);
        Task<Response<Guid>> Update(object obj, CancellationToken token);
        Task<Response<Guid>> Delete(Guid id, CancellationToken token);
        Task<Response<bool>> Delete(List<Guid> ids, CancellationToken token);
    }
}
