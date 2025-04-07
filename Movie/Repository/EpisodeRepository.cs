using Movie.Models;
using Microsoft.EntityFrameworkCore;

namespace Movie.Repository
{
    public class EpisodeRepository : IEpisodeRepository<Episode>
    {
        private readonly movieDB _context;

        public EpisodeRepository(movieDB context)
        {
            _context = context;
        }

        // Thêm một episode mới
        public async Task AddAsync(Episode entity)
        {
            await _context.Episodes.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // Xóa tất cả episode theo SeriesId
        public async Task DeleteBySeriesIdAsync(int seriesId)
        {
            var episodes = await _context.Episodes
                .Where(e => e.SeriesId == seriesId)
                .ToListAsync();

            _context.Episodes.RemoveRange(episodes);
            await _context.SaveChangesAsync();
        }
    }
}
