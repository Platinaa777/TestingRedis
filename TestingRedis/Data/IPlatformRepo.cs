using TestingRedis.Models;

namespace TestingRedis.Data;

public interface IPlatformRepo
{
    void CreatePlatform(Platform plat);
    Platform? GetPlatformById(string id);
    IEnumerable<Platform> GetAllPlatforms();
}