using Microsoft.AspNetCore.Mvc;
using Movie.Repository;
using Movie.RequestDTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Movie.Models;

namespace Movie.ControllersAdmin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminSeriesController : ControllerBase
    {
        private readonly ISeriesRepository _seriesRepository;
        private readonly string _assetsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets");

        public AdminSeriesController(ISeriesRepository seriesRepository)
        {
            _seriesRepository = seriesRepository;
        }


        // GET: api/Series
        [HttpGet]
        public async Task<ActionResult<PaginatedList<RequestSeriesDTO>>> GetSeries(
        string? search = null,
        string sortBy = "Title",          // Sắp xếp theo tên series mặc định
        string sortDirection = "asc",     // Hướng sắp xếp mặc định là tăng dần
        int page = 1,                     // Số trang
        int pageSize = 100                 // Số lượng series trên mỗi trang
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

        //// PUT: api/Series/5
        //[HttpPut("{id}")]
        //public async Task<ActionResult<RequestSeriesDTO>> UpdateSeries(
        //int id, // ID của series cần cập nhật
        //[FromQuery] RequestSeriesDTO requestSeriesDTO, // Nhận dữ liệu từ query string
        //IFormFile? posterFile, // Tệp poster
        //IFormFile? avatarFile) // Tệp avatar
        //    {
        //    // Kiểm tra ID có trùng khớp với dữ liệu request không
        //    if (id != requestSeriesDTO.SeriesId)
        //    {
        //        return BadRequest(new { Message = "ID series không khớp." });
        //    }

        //    // Lưu các file nếu có
        //    requestSeriesDTO.PosterUrl = posterFile != null ? await _seriesRepository.SaveFile(posterFile, "Posters") : requestSeriesDTO.PosterUrl;
        //    requestSeriesDTO.AvatarUrl = avatarFile != null ? await _seriesRepository.SaveFile(avatarFile, "Avatars") : requestSeriesDTO.AvatarUrl;

        //    // Cập nhật series vào cơ sở dữ liệu
        //    var updatedSeries = await _seriesRepository.UpdateSeriesAsync(id, requestSeriesDTO, requestSeriesDTO.PosterUrl, requestSeriesDTO.AvatarUrl);

        //    if (updatedSeries == null)
        //    {
        //        return NotFound(new { Message = "Không tìm thấy series để cập nhật." });
        //    }

        //    return Ok(updatedSeries); // Trả về kết quả sau khi cập nhật
        //}



        [HttpPost("AddSeries")]
        public async Task<IActionResult> AddSeries([FromForm] RequestSeriesDTO seriesDTO, IFormFile posterFile, IFormFile AvatarUrlFile)
        {
            try
            {
                if (posterFile == null || posterFile.Length == 0)
                {
                    return BadRequest(new { errors = new { posterFile = new[] { "Poster file is required" } } });
                }

                var result = await _seriesRepository.AddSeriesAsync(seriesDTO, posterFile, AvatarUrlFile);
                if (result == null) return BadRequest("Failed to add series");
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { errors = new { message = ex.Message } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new { message = "An error occurred while adding the series.", details = ex.Message } });
            }
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

        //// PUT: api/Series/update-status/5
        //[HttpPut("update-status/{id}")]
        //public async Task<IActionResult> UpdateSeriesStatus(int id, [FromBody] int status)
        //{
        //    var series = await _seriesRepository.GetSeriesByIdAsync(id);

        //    if (series == null)
        //    {
        //        return NotFound(new { Message = "Series không tồn tại." });
        //    }

        //    await _seriesRepository.UpdateSeriesStatusAsync(id, status);

        //    return Ok(new { Message = "Trạng thái series đã được cập nhật." });
        //}

        [HttpPut("UpdateSeries/{id}")]
        public async Task<IActionResult> UpdateSeries(int id, [FromForm] RequestSeriesDTO seriesDTO, IFormFile? posterFile, IFormFile? AvatarUrlFile)
        {
            if (seriesDTO == null)
            {
                return BadRequest("Invalid data");
            }

            seriesDTO.SeriesId = id;

            var result = await _seriesRepository.UpdateAsync(id, seriesDTO, posterFile, AvatarUrlFile);
            if (result == null)
            {
                return NotFound("Movie not found");
            }

            return Ok(result); // Trả về 200 OK với dữ liệu của movieDTO
        }
    }
}
