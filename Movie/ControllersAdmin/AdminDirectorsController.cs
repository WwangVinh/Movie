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

        // GET: api/Directors
        [HttpGet("List-Directors")]
        public async Task<ActionResult<IEnumerable<RequestDirectorDTO>>> GetAllDirectorsAsync(
            string? search = null,
            string sortBy = "DirectorId",
            string sortDirection = "asc",
            int page = 1,
            int pageSize = 100
            )
        {
            // Lấy danh sách actor từ repository
            var actors = await _directorRepository.GetAllDirectorsAsync(search, sortBy, sortDirection, page, pageSize);

            if (actors == null || !actors.Any())
            {
                return NotFound(new { Message = "Không tìm thấy director nào." });
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
            if (result == null) return BadRequest("lỗi khi thêm director");
            return Ok(result);
        }

        // PUT: api/Directors/Update-Director/{id}
        [HttpPut("UpdateDirector/{id}")]
        public async Task<IActionResult> UpdateDirector(
            int id,
            [FromForm] RequestDirectorDTO directorDTO,
            IFormFile? AvatarUrlFile)
        {
            var result = await _directorRepository.UpdateDirectorAsync(id, directorDTO, AvatarUrlFile);
            if (result == null) return NotFound("Director không tồn tại");
            return Ok(result);
        }

        // GET: api/Directors/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetDirector(int id)
        {
            var director = await _directorRepository.GetDirectorByIdAsync(id);
            if (director == null)
            {
                return NotFound();
            }
            return Ok(director);
        }

        // DELETE: api/Directors/Delete-Director/{id}
        [HttpDelete("DeleteDirector/{id}")]
        public async Task<IActionResult> DeleteDirector(int id)
        {
            var isDeleted = await _directorRepository.DeleteDirectorAsync(id);
            if (!isDeleted) return NotFound("Director không tồn tại để xóa");

            return Ok("Director được xóa thành công");
        }


    }
}
