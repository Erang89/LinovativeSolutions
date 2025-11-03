using Blazored.LocalStorage;
using Linovative.Frontend.Services.Interfaces;

namespace Linovative.Frontend.Services.FrontendServices
{

    public class StorageService : IStorageService
    {
        private readonly ILocalStorageService _storage;


        public StorageService(ILocalStorageService localStorageService)
        {
            _storage = localStorageService;
        }
        public async Task<T?> GetValue<T>(string key)
        {
            return await _storage.GetItemAsync<T>(key);
        }

        public async Task<string?> GetValue(string key) => await GetValue<string>(key);

        public async Task Remove(string key)
        {
            await _storage.RemoveItemAsync(key);
        }

        public async Task SetValue(string key, object value)
        {
            await _storage.SetItemAsync(key, value);
        }
    }
}
