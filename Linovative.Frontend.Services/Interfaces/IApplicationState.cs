namespace Linovative.Frontend.Services.Interfaces
{
    public interface IApplicationState
    {
        public event Action? OnChange;
        void NotifyStateChanged();
    }
}
