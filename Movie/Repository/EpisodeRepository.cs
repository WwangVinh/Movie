using Movie.Models;
using Microsoft.EntityFrameworkCore;
using Movie.RequestDTO;

namespace Movie.Repository
{
    public class EpisodeRepository : IEpisodeRepository
    {
        private readonly movieDB _context;

        public EpisodeRepository(movieDB context)
        {
            _context = context;
        }

        // Thêm một episode mới
        public async Task<RequestEpisodeDTO?> AddEpisodeAsync(RequestEpisodeDTO episodeDTO)
        {
            // Kiểm tra series có tồn tại
            var series = await _context.Series.FindAsync(episodeDTO.SeriesId);
            if (series == null) return null;

            // Tạo entity
            var episode = new Models.Episode
            {
                SeriesId = episodeDTO.SeriesId,
                EpisodeNumber = episodeDTO.EpisodeNumber,
                Title = episodeDTO.Title,
                LinkFilmUrl = episodeDTO.LinkFilmUrl
            };

            _context.Episodes.Add(episode);
            await _context.SaveChangesAsync();

            // Gán lại ID vào DTO
            episodeDTO.EpisodeId = episode.EpisodeId;

            return episodeDTO;
        }


        public async Task<Episode> GetByIdAsync(int episodeId)
        {
            return await _context.Episodes
                .Include(e => e.Series)
                .FirstOrDefaultAsync(e => e.EpisodeId == episodeId);
        }

        public async Task<List<Episode>> GetBySeriesIdAsync(int seriesId)
        {
            return await _context.Episodes
                .Include(e => e.Series)
                .Where(e => e.SeriesId == seriesId)
                .ToListAsync();
        }

        public async Task UpdateAsync(Episode episode)
        {
            _context.Episodes.Update(episode);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int episodeId)
        {
            var episode = await _context.Episodes.FindAsync(episodeId);
            if (episode != null)
            {
                _context.Episodes.Remove(episode);
                await _context.SaveChangesAsync();
            }
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
