

using Microsoft.AspNetCore.Mvc;
using Movie.Models;
using Movie.Repository;
using Movie.RequestDTO;

namespace Movie.ControllersAdmin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminMovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        public AdminMovieController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        //  Lấy danh sách phim + phân trang+ lọc + sắp xếp
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestMovieDTO>>> GetMovie(
            int pageNumber = 1,
            int pageSize = 10,
            int? categoryID = null,
            string sortBy = "Title",
            string search = ""
            )
        {
            var Movie = await _movieRepository.GetMovieAsync(pageNumber, pageSize, sortBy, search, categoryID);
            return Ok(Movie);
        }


        //  Lấy thông tin phim theo ID
        [HttpGet("Seach/{id}")]
        public async Task<ActionResult<RequestMovieDTO>> GetMovie(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound("Movie not found");
            }
            return Ok(movie);
        }

        //[HttpPost("AddFilm")]
        //public async Task<IActionResult> AddMovie([FromForm] RequestMovieDTO movieDTO, IFormFile posterFile, IFormFile AvatarUrlFile)
        //{
        //    var result = await _movieRepository.AddAsync(movieDTO, posterFile, AvatarUrlFile);
        //    if (result == null) return BadRequest("Failed to add movie");
        //    return Ok(result);
        //}

        [HttpPost("AddMovie")]
        public async Task<IActionResult> AddMovie([FromForm] RequestMovieDTO movieDTO, IFormFile posterFile, IFormFile AvatarUrlFile)
        {
            var result = await _movieRepository.AddAsync(movieDTO, posterFile, AvatarUrlFile);
            if (result == null) return BadRequest("Failed to add movie");
            return Ok(result);
        }

        //// Sửa phim
        //[HttpPut("Update/{id}")]
        //public async Task<IActionResult> UpdateMovie(int id, [FromBody] RequestMovieDTO request)
        //{
        //    if (request == null || id != request.MovieId)
        //    {
        //        return BadRequest("Invalid data");
        //    }

        //    var movie = await _movieRepository.UpdateAsync(request);
        //    if (movie == null)
        //    {
        //        return NotFound("Movie not found");
        //    }

        //    return NoContent();
        //}

        // Xoá mềm (chuyển status từ 1 -> 0)
        [HttpDelete("de/{id}")]
        public async Task<IActionResult> SoftDeleteMovie(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound("Movie not found");
            }

            await _movieRepository.SoftDeleteAsync(id);
            return NoContent();
        }


    }
}