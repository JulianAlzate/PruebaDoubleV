using BLL.Interface;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL.RN
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _database;
        private readonly TimeSpan _defaultExpiration;
        public RedisCacheService(IConfiguration configuration)
        {
            var connectionString = configuration["Redis:ConnectionString"];
            var defaultMinutes = int.Parse(configuration["Redis:DefaultCacheDurationMinutes"] ?? "30");

            var connection = ConnectionMultiplexer.Connect(connectionString);
            _database = connection.GetDatabase();
            _defaultExpiration = TimeSpan.FromMinutes(defaultMinutes);
        }
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var json = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, json, expiration ?? _defaultExpiration);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var json = await _database.StringGetAsync(key);
            if (json.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(json!);
        }

        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }
    }
}
