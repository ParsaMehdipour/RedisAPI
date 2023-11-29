using System.Text.Json;
using RedisAPI.Models;
using StackExchange.Redis;

namespace RedisAPI.DataAccess;

public class RedisPlatformRepo : IPlatformRepo
{
    private readonly IConnectionMultiplexer _connection;

    public RedisPlatformRepo(IConnectionMultiplexer connection)
    {
        _connection = connection;
    }

    public void Create(Platform platform)
    {
        ArgumentNullException.ThrowIfNull(platform, nameof(platform));

        var db = _connection.GetDatabase();

        string serializedPlatform = JsonSerializer.Serialize(platform);

        // db.StringSet(platform.Id, serializedPlatform);
        // db.SetAdd("PlatformSet",serializedPlatform);

        db.HashSet("hashPlatform", [new HashEntry(platform.Id, serializedPlatform)]);
    }

    public IEnumerable<Platform?>? GetAll()
    {
        var db = _connection.GetDatabase();

        // var completeSet = db.SetMembers("PlatformSet");

        // if(completeSet.Length > 0)
        // {
        //     var platforms = Array.ConvertAll(completeSet, val => JsonSerializer.Deserialize<Platform>(val!)).ToList();

        //     return platforms;
        // }

        var completeHash = db.HashGetAll("hashPlatform");

        if (completeHash.Length > 0)
        {
            var platforms = Array.ConvertAll(completeHash, val => JsonSerializer.Deserialize<Platform>(val.Value!)).ToList();

            return platforms;
        }

        return null;
    }

    public Platform? GetById(string key)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        var db = _connection.GetDatabase();

        // var platform = db.StringGet(key);

        var platform = db.HashGet("hashPlatform", key);

        if (string.IsNullOrEmpty(platform))
            return null;

        return JsonSerializer.Deserialize<Platform>(platform!);
    }
}