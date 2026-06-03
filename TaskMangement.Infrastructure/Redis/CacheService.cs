using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using TaskMangement.Application.Abstractions.Contracts.Persistance;

namespace TaskMangement.Infrastructure.Redis
{


    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        private static readonly JsonSerializerOptions _options =
            new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(value))
                return default;

            return JsonSerializer.Deserialize<T>(value, _options);
        }

        public async Task SetAsync<T>(string key,T value,TimeSpan expiration)
        {
            var json = JsonSerializer.Serialize(value, _options);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            await _cache.SetStringAsync(key, json, options);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
