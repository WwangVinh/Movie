using Microsoft.AspNetCore.Mvc;
using Movie.Repository;
using Movie.RequestDTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Movie.Models;

namespace Movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly ISeriesRepository _seriesRepository;
        private readonly string _assetsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets");

        public SeriesController(ISeriesRepository seriesRepository)
        {
            _seriesRepository = seriesRepository;
        }

        private async Task<string> SaveFile(IFormFile file, string subFolder)
        {
            if (file == null || file.Length == 0)
            {
                return null; // Return null if no file is provided
            }

            string directoryPath = Path.Combine(_assetsFolderPath, subFolder);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(directoryPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath; // Return the saved file path
        }

        //[HttpPost("Add-Series")]
        //public async Task<ActionResult<RequestSeriesDTO>> AddSeries(
        //     [FromBody] RequestSeriesDTO requestSeriesDTO,   // Receive image and poster data via body
        //     [FromForm] string title,                        // Separate form field for title
        //     [FromForm] string description,                  // Separate form field for description
        //     [FromForm] int rating,                          // Separate form field for rating
        //     [FromForm] string linkFilmUrl,                  // Separate form field for LinkFilmUrl
        //     [FromForm] IFormFile? posterFile,               // Handle poster file upload
        //     [FromForm] IFormFile? avatarFile                // Handle avatar file upload
        // )
        //{
        //    // Handle saving poster and avatar files
        //    string posterFilePath = null;
        //    string avatarFilePath = null;

        //    if (posterFile != null)
        //    {
        //        posterFilePath = await SaveFile(posterFile, "Posters");
        //    }

        //    if (avatarFile != null)
        //    {
        //        avatarFilePath = await SaveFile(avatarFile, "Avatars");
        //    }

        //    // Create series
        //    var createdSeries = await _seriesRepository.AddSeriesAsync(
        //        requestSeriesDTO, posterFilePath, avatarFilePath
        //    );

        //    if (createdSeries == null)
        //    {
        //        return BadRequest(new { Message = "Failed to create series." });
        //    }

        //    return CreatedAtAction(nameof(GetSeriesById), new { id = createdSeries.SeriesId }, createdSeries);
        //}



        // GET: api/Series
        [HttpGet]
        public async Task<ActionResult<PaginatedList<RequestSeriesDTO>>> GetSeries(
        string? search = null,
        string sortBy = "Title",          // Sắp xếp theo tên series mặc định
        string sortDirection = "asc",     // Hướng sắp xếp mặc định là tăng dần
        int page = 1,                    // Số trang
        int pageSize = 5                 // Số lượng series trên mỗi trang
)
        {
            // Get series from repository
            var series = await _seriesRepository.GetSeriesAsync(search, sortBy, sortDirection, page, pageSize);

            if (series == null || !series.Any()) // Now this works because PaginatedList implements IEnumerable
            {
                return NotFound(new { Message = "Không tìm thấy series nào." });
            }

            return Ok(series); // Return the PaginatedList with series data
        }


        // Method to get series by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestSeriesDTO>> GetSeriesById(int id)
        {
            var series = await _seriesRepository.GetSeriesByIdAsync(id);

            if (series == null)
            {
                return NotFound(new { Message = "Không tìm thấy series với ID này." });
            }

            return Ok(series);
        }

        // PUT: api/Series/5
        [HttpPut("{id}")]
        public async Task<ActionResult<RequestSeriesDTO>> UpdateSeries(
        int id, // ID của series cần cập nhật
        [FromQuery] RequestSeriesDTO requestSeriesDTO, // Nhận dữ liệu từ query string
        IFormFile? posterFile, // Tệp poster
        IFormFile? avatarFile) // Tệp avatar
            {
            // Kiểm tra ID có trùng khớp với dữ liệu request không
            if (id != requestSeriesDTO.SeriesId)
            {
                return BadRequest(new { Message = "ID series không khớp." });
            }

            // Lưu các file nếu có
            requestSeriesDTO.PosterUrl = posterFile != null ? await _seriesRepository.SaveFile(posterFile, "Posters") : requestSeriesDTO.PosterUrl;
            requestSeriesDTO.AvatarUrl = avatarFile != null ? await _seriesRepository.SaveFile(avatarFile, "Avatars") : requestSeriesDTO.AvatarUrl;

            // Cập nhật series vào cơ sở dữ liệu
            var updatedSeries = await _seriesRepository.UpdateSeriesAsync(id, requestSeriesDTO, requestSeriesDTO.PosterUrl, requestSeriesDTO.AvatarUrl);

            if (updatedSeries == null)
            {
                return NotFound(new { Message = "Không tìm thấy series để cập nhật." });
            }

            return Ok(updatedSeries); // Trả về kết quả sau khi cập nhật
        }




        // DELETE: api/Series/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeries(int id)
        {
            var series = await _seriesRepository.GetSeriesByIdAsync(id);

            if (series == null)
            {
                return NotFound(new { Message = "Series không tồn tại." });
            }

            await _seriesRepository.DeleteSeriesAsync(id);

            return NoContent();
        }

        // DELETE: api/Series/soft-delete/5
        [HttpDelete("soft-delete/{id}")]
        public async Task<IActionResult> SoftDeleteSeries(int id)
        {
            var series = await _seriesRepository.GetSeriesByIdAsync(id);

            if (series == null)
            {
                return NotFound(new { Message = "Series không tồn tại." });
            }

            await _seriesRepository.SoftDeleteSeriesAsync(id);

            return Ok(new { Message = "Series đã bị xóa mềm." });
        }

        // PUT: api/Series/update-status/5
        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateSeriesStatus(int id, [FromBody] int status)
        {
            var series = await _seriesRepository.GetSeriesByIdAsync(id);

            if (series == null)
            {
                return NotFound(new { Message = "Series không tồn tại." });
            }

            await _seriesRepository.UpdateSeriesStatusAsync(id, status);

            return Ok(new { Message = "Trạng thái series đã được cập nhật." });
        }
    }
}
