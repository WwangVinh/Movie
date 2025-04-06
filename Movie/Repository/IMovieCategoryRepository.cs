using System.Threading.Tasks;

namespace Movie.Repository
{
    public interface IMovieCategoryRepository<MovieCategory>
    {
        Task AddAsync(MovieCategory entity);

        Task DeleteByMovieIdAsync(int id);
    }
}