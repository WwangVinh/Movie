using Movie.Models;

namespace Movie.Repository
{
    public class SeriesCategoryRepository : ISeriesCategoryRepository<SeriesCategories>
    {
        private readonly movieDB _context;
        public SeriesCategoryRepository(movieDB context)
        {
            _context = context;
        }
        public async Task AddAsync(SeriesCategories entity)
        {
            await _context.SeriesCategories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteBySeriesIdAsync(int SeriesId)
        {
            var categories = _context.SeriesCategories.Where(mc => mc.SeriesId == SeriesId);
            _context.SeriesCategories.RemoveRange(categories);
            await _context.SaveChangesAsync();
        }
    }
}