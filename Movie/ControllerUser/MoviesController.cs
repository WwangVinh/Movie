using Microsoft.AspNetCore.Mvc;
using Movie.Repository;
using Movie.RequestDTO;

namespace Movie.ControllerUser
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }
        [HttpGet("detailMovie/{id}")]
        public async Task<IActionResult> GetDetailMovie(int id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound("Không tìm thấy phim.");
            }

            return Ok(movie);
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


    }
}
