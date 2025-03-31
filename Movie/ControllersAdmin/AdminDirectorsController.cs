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

        //// POST: api/Directors
        //[HttpPost]
        //public async Task<ActionResult> AddDirector(
        //    [FromBody] RequestDirectorDTO directorDTO,
        //    [FromForm] IFormFile? avatarFile,   // Avatar file input
        //    [FromForm] IFormFile? posterFile)   // Poster file input
        //{
        //    // Lưu tệp avatar và poster vào thư mục đã chỉ định
        //    string avatarFilePath = null;
        //    string posterFilePath = null;

        //    if (avatarFile != null)
        //    {
        //        avatarFilePath = await SaveFile(avatarFile, "Avatars");
        //    }

        //    if (posterFile != null)
        //    {
        //        posterFilePath = await SaveFile(posterFile, "Posters");
        //    }

        //    // Thêm đạo diễn vào cơ sở dữ liệu
        //    var director = await _directorRepository.AddDirectorAsync(directorDTO);

        //    return CreatedAtAction(nameof(GetDirector), new { id = director.DirectorID }, director);
        //}

        //// Lưu tệp lên thư mục tương ứng
        //private async Task<string> SaveFile(IFormFile file, string subFolder)
        //{
        //    if (file == null || file.Length == 0)
        //        return null; // Trả về null nếu không có file

        //    // Tạo thư mục nếu chưa có
        //    string directoryPath = Path.Combine(_assetsFolderPath, subFolder);
        //    if (!Directory.Exists(directoryPath))
        //    {
        //        Directory.CreateDirectory(directoryPath);
        //    }

        //    // Tạo tên file với phần mở rộng
        //    string fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //    string filePath = Path.Combine(directoryPath, fileName);

        //    // Lưu file vào thư mục
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    return filePath; // Trả về đường dẫn lưu trữ file
        //}

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
