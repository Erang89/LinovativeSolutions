using Linovative.Frontend.Services.Constans;
using Linovative.Frontend.Services.Interfaces;

namespace Linovative.Frontend.Shared.ShareServices
{
    public interface IThemeService
    {
        public bool IsDarkTheme { get; }
        Task SetDarkTheme(bool isDark, CancellationToken token = default);
    }

    public class ThemeProviderService : IThemeService
    {
        public readonly IApplicationStateService _state;
        private readonly IStorageService _storage;

        public ThemeProviderService(IApplicationStateService state, IStorageService storage)
        {
            _state = state;
            _storage = storage;
        }

        public bool IsDarkTheme { get; set; }

        public async Task SetDarkTheme(bool value, CancellationToken token = default)
        {
            IsDarkTheme = value;
            await _storage.SetValue(StorageKeys.DarkTheme, IsDarkTheme);
            _state.NotifyStateChanged();
            await Task.CompletedTask;
        }
    }
}
