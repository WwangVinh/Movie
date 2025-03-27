using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.RequestDTO;
using Movie.ResponseDTO;

namespace Movie.Repository
{
    public class MovieHomeRepository : IMovieHome
    {
        private readonly movieDB _context;
        public MovieHomeRepository(movieDB context)
        {
            _context = context;
        }
        // home
        public async Task<IEnumerable<string>> GetPostersAsync()
        {
            var posters = await _context.Movies
                .Where(m => m.Status == 1 && m.PosterUrl != null)
                .OrderByDescending(m => m.YearReleased)
                .Take(3)
                .Select(m => m.PosterUrl!)
                .ToListAsync();

            return posters;
        }
        public async Task<IEnumerable<RequestMovieDTO>> GetNewMovieAsync()
        {
            var query = _context.Movies
                .Where(m => m.Status == 1)
                .OrderByDescending(m => m.YearReleased)
                .Take(10);

            return await query.Select(movie => new RequestMovieDTO
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                AvatarUrl = movie.AvatarUrl,
            }).ToListAsync();
        }

        public async Task<IEnumerable<RequestMovieDTO>> GetHotMovieAsync()
        {
            var query = _context.Movies
                .Where(m => m.Status == 1 && m.IsHot == true)
                .OrderByDescending(m => m.Rating)
                .Take(10);

            return await query.Select(movie => new RequestMovieDTO
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                AvatarUrl = movie.AvatarUrl,
                IsHot = true
            }).ToListAsync();
        }

        public async Task<IEnumerable<RequestMovieDTO>> GetSeriesMovieAsync()
        {
            var query = _context.Series
                .Where(s => s.Status == 1)
                .OrderByDescending(s => s.YearReleased)
                .Take(10);

            return await query.Select(series => new RequestMovieDTO
            {
                MovieId = series.SeriesId,
                Title = series.Title,
                PosterUrl = series.PosterUrl,
                AvatarUrl = series.AvatarUrl,
            }).ToListAsync();
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<RequestMovieDTO>> GetActionMovieAsync()
        {
            var query = _context.Movies
                .Where(m => m.Status == 1 && m.MovieCategories.Any(mc => mc.Categories.CategoryName == "Hành động"))
                .OrderByDescending(m => m.YearReleased)
                .Take(10);

            return await query.Select(movie => new RequestMovieDTO
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                AvatarUrl = movie.AvatarUrl,
            }).ToListAsync();
        }
    }
}