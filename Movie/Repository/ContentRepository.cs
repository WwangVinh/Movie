using Microsoft.EntityFrameworkCore;
using Movie.Models;
using static Movie.RequestDTO.RequestContentDTO;

namespace Movie.Repository
{
    public class ContentRepository
    {
        private readonly movieDB _context;

        public ContentRepository(movieDB context)
        {
            _context = context;
        }
        public async Task<List<ActionContentDto>> GetActionContentAsync(
            int pageNumber,
            int pageSize,
            string sortBy,
            bool isDescending,
            string search
)
        {
            //  Movie hành động
            var movieQuery = _context.Movies
                .Include(m => m.MovieCategories)
                    .ThenInclude(mc => mc.Categories)
                .Where(m => m.MovieCategories.Any(mc => mc.Categories.CategoryName == "Hành động"));

            //  Series hành động
            var seriesQuery = _context.Series
                .Include(s => s.SeriesCategories)
                    .ThenInclude(sc => sc.Categories)
                .Where(s => s.SeriesCategories.Any(sc => sc.Categories.CategoryName == "Hành động"));

            //  Lọc theo tên
            if (!string.IsNullOrEmpty(search))
            {
                movieQuery = movieQuery.Where(m => m.Title.Contains(search));
                seriesQuery = seriesQuery.Where(s => s.Title.Contains(search));
            }

            //  Map về DTO
            var movieList = await movieQuery.Select(m => new ActionContentDto
            {
                Id = m.MovieId,
                Title = m.Title,
                AvatarUrl = m.AvatarUrl,
                Type = "Movie"
            }).ToListAsync();

            var seriesList = await seriesQuery.Select(s => new ActionContentDto
            {
                Id = s.SeriesId,
                Title = s.Title,
                AvatarUrl = s.AvatarUrl,
                Type = "Series"
            }).ToListAsync();

            //  Gộp & sắp xếp
            var combined = movieList.Concat(seriesList).AsQueryable();

            combined = sortBy.ToLower() switch
            {
                "title" => isDescending ? combined.OrderByDescending(c => c.Title) : combined.OrderBy(c => c.Title),
                _ => combined.OrderBy(c => c.Title)
            };

            return combined
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

    }
}