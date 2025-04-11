using Microsoft.AspNetCore.Mvc;
using Movie.Models;
using Movie.Repository;
using Movie.RequestDTO;
using Movie.ResponseDTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.ControllersAdmin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminActorController : ControllerBase
    {
        private readonly IActorRepository _actorRepository;

        // Constructor to inject the repository
        public AdminActorController(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }

        // GET: api/Actors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestActorDTO>>> GetActors(
            string? search = null,           // Tìm kiếm theo tên hoặc mô tả actor
            string sortBy = "ActorId",       // Sắp xếp theo tên actor mặc định
            string sortDirection = "asc",    // Hướng sắp xếp mặc định là tăng dần
            int page = 1,                    // Số trang mặc định là trang 1
            int pageSize = 10                 // Số lượng actor trên mỗi trang
        )
        {
            // Lấy danh sách actor từ repository
            var actors = await _actorRepository.GetActorsAsync(search, sortBy, sortDirection, page, pageSize);

            if (actors == null || !actors.Any())
            {
                return NotFound(new { Message = "Không tìm thấy actor nào." });
            }

            // Trả về kết quả
            return Ok(actors);
        }

        // GET: api/Actors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestActorDTO>> GetActorById(int id)
        {
            var actor = await _actorRepository.GetActorByIdAsync(id);
            if (actor == null)
            {
                return NotFound(new { Message = "Actor not found." });
            }

            return Ok(actor);
        }

        [HttpPost("AddActor")]
        public async Task<IActionResult> AddActor( [FromForm] RequestActorDTO actorDTO, IFormFile? AvatarUrlFile)
        {
            var result = await _actorRepository.AddActorAsync(actorDTO, AvatarUrlFile);
            if (result == null)
                return BadRequest("Lỗi khi thêm actor");

            return Ok(result);
        }

        // PUT: api/Actors/UpdateActor/{id}
        [HttpPut("UpdateActor/{id}")]
        public async Task<IActionResult> UpdateActor(
            int id,
            [FromForm] RequestActorDTO actorDTO,
            IFormFile? AvatarUrlFile)
        {
            var result = await _actorRepository.UpdateActorAsync(id, actorDTO, AvatarUrlFile);
            if (result == null)
                return NotFound("Actor not found");

            return Ok(result);
        }


        // DELETE: api/Actors/5
        [HttpDelete("DeleteActor/{id}")]
        public async Task<IActionResult> DeleteActor(int id)
        {
            var isDeleted = await _actorRepository.DeleteActorAsync(id);
            if(!isDeleted) return NotFound("Actor không tồn tại để xóa");

            return Ok("Actorl được xóa thành công");
        }
    }
}
