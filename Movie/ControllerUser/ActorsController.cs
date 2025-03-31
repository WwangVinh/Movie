using Microsoft.AspNetCore.Mvc;
using Movie.Repository;

namespace Movie.ControllerUser
{
    [ApiController]
    [Route("api/actors")]
    public class ActorController : ControllerBase
    {
        private readonly IActorRepository _actorRepository;

        public ActorController(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }

        // GET: api/actors/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActorById(int id)
        {
            var actor = await _actorRepository.GetActorByIdAsync(id);
            if (actor == null)
            {
                return NotFound(new { message = "Actor not found!" });
            }
            return Ok(actor);
        }
    }
}
