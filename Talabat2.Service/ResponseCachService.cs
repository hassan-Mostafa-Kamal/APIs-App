using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat2.core.Services;

namespace Talabat2.Service
{
    public class ResponseCachService : IResponseCachService
    {
        private readonly IDatabase _database;
        public ResponseCachService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
            
        }
        public async Task CashResponseAsync(string cashKey, object response, TimeSpan timeToLive)
        {
           if (response == null) { return; }

            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
           var serializedResponse = JsonSerializer.Serialize(response, options);

            await _database.StringSetAsync(cashKey, serializedResponse, timeToLive);
        }


        public async Task<string> GetCashedResponseAsync(string cashKey)
        {
            var cashedResponse = await _database.StringGetAsync(cashKey);

            if (cashedResponse.IsNullOrEmpty)
            {
                return null;
            }
            return cashedResponse;
        }
    }
}
