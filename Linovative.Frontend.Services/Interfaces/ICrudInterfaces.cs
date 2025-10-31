using Linovative.Frontend.Services.Models;

namespace Linovative.Frontend.Services.Interfaces
{
    public interface ICrudInterfaces
    {
        Task<Response> Create(object obj, CancellationToken token);
        Task<Response> Update(object obj, CancellationToken token);
        Task<Response> Delete(Guid id, CancellationToken token);
        Task<Response> Delete(object ids, CancellationToken token);
    }
}
