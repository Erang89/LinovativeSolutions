namespace Linovative.Frontend.Services.Interfaces
{
    public interface IUnauthorizeHandler
    {
        Task Handle(CancellationToken token = default);
    }
}
