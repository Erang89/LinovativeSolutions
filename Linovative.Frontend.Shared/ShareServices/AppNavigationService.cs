using Linovative.Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Linovative.Frontend.Shared.ShareServices
{
    public class AppNavigationService : IAppNavigationManager
    {
        private readonly NavigationManager _navigationManager;
        public AppNavigationService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public string Uri => _navigationManager.Uri;
        void IAppNavigationManager.NavigateTo(string url, bool forceLoad) => _navigationManager.NavigateTo(url, forceLoad);
    }
}
