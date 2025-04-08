

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

            if (Movie == null || !Movie.Any()) // Now this works because PaginatedList implements IEnumerable
            {
                return NotFound(new { Message = "Không tìm thấy Movie nào." });
            }

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

        [HttpPut("UpdateMovie/{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromForm] RequestMovieDTO movieDTO, IFormFile? posterFile, IFormFile? AvatarUrlFile)
        {
            if (movieDTO == null)
            {
                return BadRequest("Invalid data");
            }

            movieDTO.MovieId = id;

            var result = await _movieRepository.UpdateAsync(id, movieDTO, posterFile, AvatarUrlFile);
            if (result == null)
            {
                return NotFound("Movie not found");
            }

            return Ok(result); // Trả về 200 OK với dữ liệu của movieDTO
        }
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