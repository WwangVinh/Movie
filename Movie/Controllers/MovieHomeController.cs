
using Microsoft.AspNetCore.Mvc;

using Movie.Repository;
using Movie.RequestDTO;
using Movie.ResponseDTO;

namespace Movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieHomeController : ControllerBase
    {
        private readonly IMovieHome _movieRepository;

        public MovieHomeController(IMovieHome movieRepository)
        {
            _movieRepository = movieRepository;
        }
        // Lấy poster
        [HttpGet]
        public async Task<IActionResult> GetPosterAsync()
        {
            var Movie = await _movieRepository.GetPostersAsync();
            return Ok(Movie);
        }
        // Lấy danh sách phim mới
        [HttpGet("new")]
        public async Task<IActionResult> GetNewMovie()
        {
            var Movie = await _movieRepository.GetNewMovieAsync();
            return Ok(Movie);
        }

        // Lấy danh sách phim hot
        [HttpGet("hot")]
        public async Task<IActionResult> GetHotMovie()
        {
            var Movie = await _movieRepository.GetHotMovieAsync();
            return Ok(Movie);
        }

        // Lấy danh sách phim bộ
        [HttpGet("series")]
        public async Task<IActionResult> GetSeriesMovie()
        {
            var Movie = await _movieRepository.GetSeriesMovieAsync();
            return Ok(Movie);
        }

        // Lấy danh sách phim hành động
        [HttpGet("action")]
        public async Task<IActionResult> GetActionMovie()
        {
            var Movie = await _movieRepository.GetActionMovieAsync();
            return Ok(Movie);
        }
    }
}