using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movie.Models;

namespace Movie.Repository
{
    public class MovieActorRepository : IMovieActorRepository<MovieActor>
    {

        private readonly movieDB _context;
        public MovieActorRepository(movieDB context)
        {
            _context = context;
        }
        public async Task<int> CountAsync()
        {
            return await _context.Movies.CountAsync();
        }
        public async Task AddAsync(MovieActor entity)
        {
            await _context.MovieActor.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteByMovieIdAsync(int MovieId)
        {
            var actors = _context.MovieActor.Where(mc => mc.MovieId == MovieId);
            _context.MovieActor.RemoveRange(actors);
            await _context.SaveChangesAsync();
        }
    }
}