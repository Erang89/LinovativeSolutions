using Linovative.Frontend.Services.Interfaces;

namespace Linovative.Frontend.Services.FrontendServices
{
    public interface ITopBarService
    {
        public string? Title { get; }
        public void SetTitle(string title);
    }
    public class TopBarService : ITopBarService
    {
        IApplicationStateService _state;
        public TopBarService(IApplicationStateService state)
        {
            _state = state;
        }

        public string? Title { get; private set; }
        public void SetTitle(string title)
        {
            Title = title;
            _state.NotifyStateChanged();
        }
    }
}
