using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestingRedis.Data;
using TestingRedis.Models;

namespace TestingRedis.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _repo;

    public PlatformsController(IPlatformRepo repo)
    {
        _repo = repo;
    }
    
    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<Platform> GetPlatformById(string id)
    {
        var platform = _repo.GetPlatformById(id);

        if (platform != null)
        {
            return Ok(platform);
        }

        return NotFound();
    }

    [HttpPost]
    public ActionResult<Platform> CreatePlatform(Platform obj)
    {
        _repo.CreatePlatform(obj);

        return CreatedAtRoute(
            nameof(GetPlatformById),
    new { Id = obj.Id },
        obj);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Platform>> GetAll()
    {
        return Ok(_repo.GetAllPlatforms());
    }
}