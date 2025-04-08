using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movie.RequestDTO;
using Movie.Repository;
using System.IO;
using System.Threading.Tasks;
using Movie.Models;

namespace Movie.ControllersAdmin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDirectorsController : ControllerBase
    {
        private readonly IDirectorsRepository _directorRepository;
        private readonly string _assetsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets");

        public AdminDirectorsController(IDirectorsRepository directorRepository)
        {
            _directorRepository = directorRepository;
        }

        // GET: api/Actors
        [HttpGet("List-Actors")]
        public async Task<ActionResult<IEnumerable<RequestDirectorDTO>>> GetAllDirectorsAsync(
            string? search = null,
            string sortBy = "DirectorId",
            string sortDirection = "asc",
            int page = 1,
            int pageSize = 10
            )
        {
            // Lấy danh sách actor từ repository
            var actors = await _directorRepository.GetAllDirectorsAsync(search, sortBy, sortDirection, page, pageSize);

            if (actors == null || !actors.Any())
            {
                return NotFound(new { Message = "Không tìm thấy actor nào." });
            }

            // Trả về kết quả
            return Ok(actors);
        }

        // POST: api/Directors/Add-Director
        [HttpPost("AddDirector")]
        public async Task<IActionResult> AddDirector(
            [FromForm] RequestDirectorDTO directorDTO, IFormFile AvatarUrlFile)
        {
            var result = await _directorRepository.AddDirectorAsync(directorDTO, AvatarUrlFile);
            if (result == null) return BadRequest("Failed to add director");
            return Ok(result);
        }

        //// GET: api/Directors/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult> GetDirector(int id)
        //{
        //    var director = await _directorRepository.GetDirectorByIdAsync(id);
        //    if (director == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(director);
        //}
    }
}
