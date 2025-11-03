using Linovative.Frontend.Services.Interfaces;

namespace Linovative.Frontend.Services.FrontendServices.BaseServices
{
    public class ApplicationStateService : IApplicationStateService
    {
        public event Action? OnChange;

        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
