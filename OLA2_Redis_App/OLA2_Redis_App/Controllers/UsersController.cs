using OLA2_Redis_App.Services;

namespace OLA2_Redis_App.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly RedisService _redis;

    public UsersController(RedisService redis)
    {
        _redis = redis;
    }

    [HttpPost("{key}")]
    public async Task<IActionResult> CreateUser(string key, [FromQuery] string value, [FromQuery] int? ttlSeconds = null)
    {
        TimeSpan? ttl = ttlSeconds.HasValue ? TimeSpan.FromSeconds(ttlSeconds.Value) : null;
        await _redis.CreateUserAsync(key, value, ttl);
        return Ok($"User {key} created");
    }

    [HttpGet("{key}")]
    public async Task<IActionResult> GetUser(string key)
    {
        var value = await _redis.GetUserAsync(key);
        return value is null ? NotFound() : Ok(value);
    }

    [HttpPut("{key}")]
    public async Task<IActionResult> UpdateUser(string key, [FromQuery] string value)
    {
        var updated = await _redis.UpdateUserAsync(key, value);
        return updated ? Ok("Updated") : NotFound();
    }

    [HttpDelete("{key}")]
    public async Task<IActionResult> DeleteUser(string key)
    {
        var deleted = await _redis.DeleteUserAsync(key);
        return deleted ? Ok("Deleted") : NotFound();
    }

    [HttpGet("{key}/ttl")]
    public async Task<IActionResult> GetTTL(string key)
    {
        var ttl = await _redis.GetUserTTLAsync(key);
        return Ok($"TTL (seconds): {ttl}");
    }
}
