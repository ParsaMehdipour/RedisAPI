using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RedisAPI.DataAccess;
using RedisAPI.Models;

namespace RedisAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _platformRepo;

    public PlatformsController(IPlatformRepo platformRepo)
    {
        _platformRepo = platformRepo;
    }

    [HttpGet(Name = "GetAll")]
    public ActionResult<IEnumerable<Platform>> GetAll(){

        return Ok(_platformRepo.GetAll());

    }

    [HttpGet("{id}", Name = "GetById")]
    public ActionResult<Platform> GetById(string id){

        Platform? platform = _platformRepo.GetById(id);

        if(platform is null)
            return NotFound();

        return Ok(platform);

    }

    [HttpPost(Name ="Create")]
    public ActionResult<Platform> Create(Platform platform){

        _platformRepo.Create(platform);

        return CreatedAtRoute(nameof(GetById), new { id = platform.Id }, platform);
    }
}