using Linovative.Frontend.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Linovative.Frontend.Shared.ShareServices
{
    public class AppNavigationService : IAppNavigationService
    {
        private readonly NavigationManager _navigationManager;
        public AppNavigationService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public string Uri => _navigationManager.Uri;
        void IAppNavigationService.NavigateTo(string url, bool forceLoad) => _navigationManager.NavigateTo(url, forceLoad);
    }
}
