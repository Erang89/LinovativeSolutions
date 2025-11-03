using Linovative.Frontend.Services.Interfaces;

namespace Linovative.Frontend.Shared.ShareServices
{
    public interface IMainMenuService
    {
        public bool IsShow { get; }
        public void Toggle();
    }

    public class MainMenuService : IMainMenuService
    {
        private readonly IApplicationStateService _state;
        public MainMenuService(IApplicationStateService state)
        {
            _state = state;
        }

        public bool IsShow { get; set; }

        public void Toggle()
        {
            IsShow = !IsShow;
            _state.NotifyStateChanged();
        }
    }
}
