namespace Linovative.Frontend.Services.Interfaces
{
    public interface IAppNavigationService
    {
        string Uri { get; }
        void NavigateTo(string url, bool forceLoad = default);
    }
}
