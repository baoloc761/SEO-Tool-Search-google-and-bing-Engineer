using Microsoft.Extensions.Caching.Memory;
using SearchEngineApp.Models;

namespace SearchEngineApp.Services
{
    public class SearchCacheService
    {
        private readonly IMemoryCache _cache;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

        public SearchCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public SearchResult GetCachedResult(string keywords)
        {
            return _cache.TryGetValue(keywords, out SearchResult cachedResult) ? cachedResult : null;
        }

        public void SetCachedResult(string keywords, SearchResult result)
        {
            _cache.Set(keywords, result, CacheDuration);
        }
    }
}

