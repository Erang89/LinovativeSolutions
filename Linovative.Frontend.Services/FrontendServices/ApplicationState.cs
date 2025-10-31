using Linovative.Frontend.Services.Interfaces;

namespace Linovative.Frontend.Services.FrontendServices
{
    public class ApplicationState : IApplicationState
    {
        public event Action? OnChange;

        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
