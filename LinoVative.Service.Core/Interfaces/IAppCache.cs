namespace LinoVative.Service.Core.Interfaces
{
    public interface IAppCache
    {
        public Task<T?> Get<T>(string key, Func<Task<T?>> getObject);
    }
}
