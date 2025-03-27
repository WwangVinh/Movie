using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.Repository;

namespace Movie.ControllerWeb
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
        [HttpGet("MovieHot")]
        public async Task<IActionResult> GetMovieHot(int id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound("Không tìm thấy phim.");
            }

            return Ok(movie);
        }
    }
}