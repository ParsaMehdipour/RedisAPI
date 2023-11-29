using RedisAPI.Models;

namespace RedisAPI.DataAccess;

public interface IPlatformRepo
{
    void Create(Platform platform);
    Platform? GetById(string key);
    IEnumerable<Platform?>? GetAll();
}