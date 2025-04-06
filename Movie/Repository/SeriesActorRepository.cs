using Microsoft.EntityFrameworkCore;
using Movie.Models;

namespace Movie.Repository
{
    public class SeriesActorRepository : ISeriesActorRepository<SeriesActors>
    {
        private readonly movieDB _context;
        public SeriesActorRepository(movieDB context)
        {
            _context = context;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Series.CountAsync();
        }

        public async Task AddAsync(SeriesActors entity)
        {
            await _context.SeriesActors.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteBySeriesIdAsync(int SeriesId)
        {
            var actors = _context.SeriesActors.Where(mc => mc.SeriesId == SeriesId);
            _context.SeriesActors.RemoveRange(actors);
            await _context.SaveChangesAsync();
        }
    }
}
