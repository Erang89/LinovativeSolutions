using Linovative.Frontend.Services.Interfaces;
using Blazored.LocalStorage;

namespace Linovative.Frontend.Services.Commons
{
    public sealed class LanguageService : ILanguageProvider
    {
        private const string Key = "BlazorCulture";
        private readonly ILocalStorageService _storage;

        public LanguageService(ILocalStorageService storage) => _storage = storage;

        public async Task<string?> GetLanguage() => await _storage.GetItemAsStringAsync(Key);

        public async Task SetLanguage(string cultureName) => await _storage.SetItemAsStringAsync(Key, cultureName);

        public string DefaultCulture => "en";
    }
}
