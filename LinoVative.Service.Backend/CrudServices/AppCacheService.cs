using LinoVative.Service.Core.Interfaces;

namespace LinoVative.Service.Backend.CrudServices
{
    public class AppCacheService : IAppCache, IScoopService
    {
        private readonly Dictionary<string, object?> cache = new Dictionary<string, object?>();

        public async Task<T?> Get<T>(string key, Func<Task<T?>> getObject)
        {
            if(cache.ContainsKey(key))
            {
                return (T?)cache[key];
            }

            var obj = await getObject();
            cache.Add(key, obj);
            return obj;
        }
    }
}
