using System.Threading.Tasks;

namespace Movie.Repository
{
    public interface IMovieActorRepository<MovieActor>
    {
        Task AddAsync(MovieActor entity);
        Task DeleteByMovieIdAsync(int id);
    }
}