using Microsoft.AspNetCore.Mvc;
using Movie.Repository;

namespace Movie.ControllerUser
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorsController : ControllerBase
    {
        private readonly IDirectorsRepository _directorRepository;

        public DirectorsController(IDirectorsRepository directorRepository)
        {
            _directorRepository = directorRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDirectorById(int id)
        {
            var director = await _directorRepository.GetDirectorByIdAsync(id);
            if (director == null)
                return NotFound(new { message = "Director not found" });

            return Ok(director);
        }
    }
}
