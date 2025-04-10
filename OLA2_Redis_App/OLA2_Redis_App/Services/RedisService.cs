namespace OLA2_Redis_App.Services;

using StackExchange.Redis;
using System.Text.Json;

public class RedisService
{
    private readonly IDatabase _db;

    public RedisService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task CreateUserAsync(string username, string name, TimeSpan? ttl = null)
    {
        var key = $"user:{username}:name";
        await _db.StringSetAsync(key, name, ttl);
    }

    public async Task<string?> GetUserAsync(string username)
    {
        var key = $"user:{username}:name";
        return await _db.StringGetAsync(key);
    }

    public async Task<bool> UpdateUserAsync(string username, string newName)
    {
        var key = $"user:{username}:name";
        return await _db.StringSetAsync(key, newName);
    }

    public async Task<bool> DeleteUserAsync(string username)
    {
        var key = $"user:{username}:name";
        return await _db.KeyDeleteAsync(key);
    }

    public async Task<double> GetUserTTLAsync(string username)
    {
        var key = $"user:{username}:name";
        return (await _db.KeyTimeToLiveAsync(key))?.TotalSeconds ?? -1;
    }
}
