namespace Linovative.Frontend.Services.Interfaces
{
    public interface IUnauthorizeHandlerService
    {
        Task Handle(CancellationToken token = default);
    }
}
