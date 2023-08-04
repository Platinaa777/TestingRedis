using System.Text.Json;
using StackExchange.Redis;
using TestingRedis.Models;

namespace TestingRedis.Data;

public class RedisPlatform : IPlatformRepo
{
    private readonly IConnectionMultiplexer _redis;

    public RedisPlatform(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }
    
    public void CreatePlatform(Platform plat)
    {
        if (plat is null)
        {
            throw new ArgumentException(nameof(plat));
        }
        
        var db = _redis.GetDatabase();

        var serialPlat = JsonSerializer.Serialize(plat);
    
        // create unique key in redis 
        // db.StringSet(plat.Id, serialPlat);
        // // add to list(set, dict) in redis
        // db.SetAdd("PlatformSet", serialPlat);

        db.HashSet("hashplatform", new HashEntry[]
        {
            new HashEntry(plat.Id, serialPlat),
        });
    }

    public Platform? GetPlatformById(string id)
    {
        var db = _redis.GetDatabase();

        // var plat = db.StringGet(id);

        var plat = db.HashGet("hashplatform", id);
        
        if (!string.IsNullOrEmpty(plat))
        {
            return JsonSerializer.Deserialize<Platform>(plat);
        }

        return null;
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        var db = _redis.GetDatabase();

        // var completeSet = db.SetMembers("PlatformSet");

        var completeSet = db.HashGetAll("hashplatform");
        
        if (completeSet.Length > 0)
        {
            var arr = Array.ConvertAll(
                completeSet, 
        val => JsonSerializer.Deserialize<Platform>(val.Value))
                .ToList();

            return arr;
        }

        return null;
    }
}