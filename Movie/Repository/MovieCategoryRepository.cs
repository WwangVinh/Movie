
using System.Linq;
using System.Threading.Tasks;
using Movie.Models;

namespace Movie.Repository
{
    public class MovieCategoryRepository : IMovieCategoryRepository<MovieCategories>
    {
        private readonly movieDB _context;
        public MovieCategoryRepository(movieDB context)
        {
            _context = context;
        }
        public async Task AddAsync(MovieCategories entity)
        {
            await _context.MovieCategories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteByMovieIdAsync(int MovieId)
        {
            var categories = _context.MovieCategories.Where(mc => mc.MovieId == MovieId);
            _context.MovieCategories.RemoveRange(categories);
            await _context.SaveChangesAsync();
        }
    }
}
