// 代码生成时间: 2025-10-25 09:55:27
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading.Tasks;

namespace CachingStrategyApp
{
    public class CacheStrategy<TContext, TEntity> where TContext : DbContext where TEntity : class
    {
        private readonly IMemoryCache _memoryCache;
        private readonly TContext _dbContext;
        private readonly string _cacheKey;

        public CacheStrategy(TContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
            // Define a unique cache key for the entity
            _cacheKey = typeof(TEntity).Name;
        }

        /*
         * Retrieves data from the cache if available, otherwise fetches from the database.
         * The data is then cached for future requests.
         */
        public async Task<List<TEntity>> GetDataAsync()
        {
            if (_memoryCache.TryGetValue(_cacheKey, out List<TEntity> cachedData))
            {
                Console.WriteLine("Data retrieved from cache.");
                return cachedData;
            }
            else
            {
                try
                {
                    Console.WriteLine("Fetching data from database.");
                    var data = await _dbContext.Set<TEntity>().ToListAsync();
                    // Set cache options
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    {
                        // Set absolute expiration to 20 minutes.
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
                    };
                    // Save data in cache.
                    _memoryCache.Set(_cacheKey, data, cacheEntryOptions);
                    return data;
                }
                catch (Exception ex)
                {
                    // Log the error and throw for further handling
                    Console.WriteLine($"Error fetching data: {ex.Message}");
                    throw;
                }
            }
        }

        /*
         * Clears the cache for the specified entity type.
         */
        public void ClearCache()
        {
            _memoryCache.Remove(_cacheKey);
            Console.WriteLine($"Cache for {_cacheKey} cleared.");
        }

        // Additional methods can be added here for other caching strategies or operations.
    }
}
