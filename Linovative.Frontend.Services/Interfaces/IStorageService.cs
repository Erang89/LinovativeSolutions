namespace Linovative.Frontend.Services.Interfaces
{
    public interface IStorageService
    {
        Task SetValue(string key, object value);
        Task<T?> GetValue<T>(string key);
        Task<string?> GetValue(string key);
        Task Remove(string key);
    }
}
