using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace Assignment.Storage
{
    public class CachedStorage : ICachedStorage
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IStorage _storage;
        private readonly MemoryCacheEntryOptions _options;

        public CachedStorage(IStorage storage)
        {
            _memoryCache = new MemoryCache(
                new MemoryCacheOptions
                {
                    SizeLimit = 1024,
                });
            _storage = storage;
            _options = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(3))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(20))
                .SetSize(1024 * 1024);
        }

        public JsonElement? Load(Guid id)
        {
            if (_memoryCache.TryGetValue(id, out JsonElement? result))
            {
                return result;
            }

            result = _storage.Load(id);
            _memoryCache.Set(id, result, _options);
            return result;
        }

        public void Save(Guid id, JsonElement o)
        {
            _storage.Save(id, o);
            _memoryCache.Set(id, o, _options);
        }
    }
}
