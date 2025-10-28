using Linovative.Frontend.Services.Interfaces;

namespace Linovative.Frontend.Shared.ShareServices
{
    public class UnauthorizedHandlerService : IUnauthorizeHandler
    {
        private readonly IAppNavigationManager _nav;
        public UnauthorizedHandlerService(IAppNavigationManager nav)
        {
            _nav = nav;
        }
        public Task Handle(CancellationToken token = default)
        {
            _nav.NavigateTo("/logout");
            return Task.CompletedTask;
        }
    }
}
