using Microsoft.AspNetCore.Mvc;
using Movie.Models;
using Movie.Repository;
using Movie.RequestDTO;
using Movie.ResponseDTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly IActorRepository _actorRepository;

        // Constructor to inject the repository
        public ActorController(IActorRepository actorRepository)
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
            int pageSize = 5                 // Số lượng actor trên mỗi trang
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

        //// POST: api/Actors
        //[HttpPost]
        //public async Task<ActionResult<RequestActorDTO>> AddActor(
        //    [FromForm] string nameAct,
        //    [FromForm] string description,
        //    [FromForm] string nationality,
        //    [FromForm] string professional,
        //    [FromForm] string avatarUrl,
        //    [FromForm] int status,
        //    IFormFile? avatarFile)  // Nhận tệp ảnh
        //{
        //    // Kiểm tra nếu có tệp avatar thì lưu nó
        //    string avatarPath = null;
        //    if (avatarFile != null)
        //    {
        //        avatarPath = await _actorRepository.SaveFile(avatarFile, "Avatars");
        //    }

        //    // Tạo đối tượng actor với dữ liệu từ form
        //    var actorDTO = new RequestActorDTO
        //    {
        //        NameAct = nameAct,
        //        Description = description,
        //        Nationality = nationality,
        //        Professional = professional,
        //        AvatarUrl = avatarPath ?? avatarUrl,  // Sử dụng avatarUrl nếu không có file
        //        Status = status
        //    };

        //    // Gọi repository để thêm actor vào cơ sở dữ liệu
        //    var createdActor = await _actorRepository.AddActorAsync(actorDTO);

        //    if (createdActor == null)
        //    {
        //        return BadRequest(new { Message = "Failed to create actor." });
        //    }

        //    return CreatedAtAction(nameof(GetActorById), new { id = createdActor.ActorId }, createdActor);
        //}

        // PUT: api/Actors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActor(int id,
            [FromForm] string nameAct,
            [FromForm] string description,
            [FromForm] string nationality,
            [FromForm] string professional,
            [FromForm] string avatarUrl,
            IFormFile? avatarFile)
        {
            // Kiểm tra xem Actor có tồn tại hay không
            var existingActor = await _actorRepository.GetActorByIdAsync(id);
            if (existingActor == null)
            {
                return NotFound(new { Message = "Actor không tồn tại." });
            }

            // Lưu file ảnh nếu có
            if (avatarFile != null)
            {
                var avatarPath = await _actorRepository.SaveFile(avatarFile, "Avatars");
                existingActor.AvatarUrl = avatarPath;
            }
            else
            {
                existingActor.AvatarUrl = avatarUrl; // Chỉ cập nhật avatarUrl nếu không có file ảnh mới
            }

            // Cập nhật thông tin Actor
            existingActor.NameAct = nameAct;
            existingActor.Description = description;
            existingActor.Nationality = nationality;
            existingActor.Professional = professional;

            // Gọi repository để cập nhật actor
            var updatedActor = await _actorRepository.UpdateActorAsync(id, existingActor);

            if (updatedActor == null)
            {
                return BadRequest(new { Message = "Cập nhật actor thất bại." });
            }

            return Ok(updatedActor);  // Trả về actor đã được cập nhật
        }

        // DELETE: api/Actors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(int id)
        {
            await _actorRepository.DeleteActorAsync(id);
            return NoContent();
        }
    }
}
