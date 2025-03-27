using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.Repository;
using Movie.RequestDTO;

namespace Movie.ControllerWeb
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly ISeriesRepository _seriesRepository;

        public SeriesController(ISeriesRepository seriesRepository)
        {
            _seriesRepository = seriesRepository;
        }

        [HttpGet("detailSeries/{id}")]
        public async Task<ActionResult<RequestSeriesDTO>> GetDetailMovie(int id)
        {
            var movie = await _seriesRepository.GetSeriesByIdAsync(id);
            if (movie == null)
            {
                return NotFound("Không tìm thấy phim.");
            }

            return Ok(movie);
        }
    }
}