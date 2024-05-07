using Microsoft.AspNetCore.Cors;
using StackExchange.Redis;
using System.Text.Json;

namespace DeliveryVHGP.Services
{
    [EnableCors("MyPolicy")]
    public class CacheService : ICacheService
    {
        private IDatabase _cacheDb;
        
        public CacheService()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _cacheDb = redis.GetDatabase();
        }
        public T GetData<T>(string key)
        {
            var value = _cacheDb.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;
        }
        public List<T> GetListData<T>(string key)
        {
            var values = _cacheDb.ListRange(key);

            if (values.Length == 0)
            {
                return default;
            }

            var result = new List<T>();
            foreach (var value in values)
            {
                result.Add(JsonSerializer.Deserialize<T>(value));
            }

            return result;
        }
        public object RemoveData(string key)
        {
            var exist = _cacheDb.KeyExists(key);
            if (exist)
            {
                return _cacheDb.KeyDelete(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expirtyTime = expirationTime.DateTime.Subtract(DateTime.Now);
            return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expirtyTime);
        }
        public bool SetListData<T>(string key, List<T> values, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);

            foreach (var value in values)
            {
                _cacheDb.ListRightPush(key, JsonSerializer.Serialize(value));
            }

            return _cacheDb.KeyExpire(key, expiryTime);
        }

    }
}
