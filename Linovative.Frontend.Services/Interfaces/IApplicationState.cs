namespace Linovative.Frontend.Services.Interfaces
{
    public interface IApplicationStateService
    {
        public event Action? OnChange;
        void NotifyStateChanged();
    }
}
