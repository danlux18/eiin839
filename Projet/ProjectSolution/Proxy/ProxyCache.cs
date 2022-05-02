using System;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace Proxy
{
    public class ProxyCache<T> 
    {
        private ObjectCache cache;
        private readonly Func<string, Task<T>> itemCreation;
        public ProxyCache(Func<string, Task<T>> itemCrea)
        {
            cache = MemoryCache.Default;
            itemCreation = itemCrea;
        }

        public async Task<T> Get(string CacheItemName)
        {
            Console.WriteLine("Get without expiration : "+CacheItemName);
            var item = cache.Get(CacheItemName);
            if (item == null)
            {
                item = await itemCreation(CacheItemName);
                cache.Set(CacheItemName, item, null);
            }
            return (T) item;
        }

        public async Task<T> Get(string CacheItemName, double dt_seconds)
        {
            Console.WriteLine("Get with "+dt_seconds+" seconds : " + CacheItemName);
            var item = cache.Get(CacheItemName);
            if (item == null)
            {
                item = await itemCreation(CacheItemName);
                var cacheItemPolicy = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(dt_seconds),

                };
                cache.Set(CacheItemName, item, cacheItemPolicy);
            }
            return (T) item;
        }

        public async Task<T> Get(string CacheItemName, DateTimeOffset dt)
        {
            var item = cache.Get(CacheItemName);
            if (item == null)
            {
                item = await itemCreation(CacheItemName);
                var cacheItemPolicy = new CacheItemPolicy
                {
                    AbsoluteExpiration = dt,

                };
                cache.Set(CacheItemName, item, cacheItemPolicy);
            }
            return (T) item;
        }
    }
}
