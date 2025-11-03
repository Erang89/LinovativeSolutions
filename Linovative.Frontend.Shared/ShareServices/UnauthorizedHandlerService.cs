using Linovative.Frontend.Services.Interfaces;

namespace Linovative.Frontend.Shared.ShareServices
{
    public class UnauthorizedHandlerService : IUnauthorizeHandlerService
    {
        private readonly IAppNavigationService _nav;
        public UnauthorizedHandlerService(IAppNavigationService nav)
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
